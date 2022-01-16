using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IdentityServerOptions _options;
        private readonly ITokenResponseGenerator _generator;
        private readonly ISecretsListParser _secretsParser;
        private readonly IScopeValidator _scopeValidator;
        private readonly ISecretsListValidator _secretsValidator;
        private readonly IResourceValidator _resourceValidator;
        private readonly IGrantTypeValidator _grantTypeValidator;

        public TokenEndpoint(
            IClientStore clients,
            IResourceStore resources,
            ISecretsListParser secretsParser,
            IdentityServerOptions options,
            ITokenResponseGenerator generator,
            IScopeValidator scopeValidator,
            ISecretsListValidator secretsValidator,
            IResourceValidator resourceValidator,
            IGrantTypeValidator grantTypeValidator)
        {
            _clients = clients;
            _options = options;
            _resources = resources;
            _generator = generator;
            _secretsParser = secretsParser;
            _scopeValidator = scopeValidator;
            _secretsValidator = secretsValidator;
            _resourceValidator = resourceValidator;
            _grantTypeValidator = grantTypeValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            #region ValidateRequest
            if (!_options.Endpoints.EnableTokenEndpoint)
            {
                return MethodNotAllowed();
            }
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidRequest, "Invalid context type");
            }
            #endregion

            #region Validate ClientSecret
            var clientSecret = await _secretsParser.ParseAsync(context);
            if (clientSecret == null)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "No client credentials found");
            }
            var client = await _clients.GetAsync(clientSecret.Id);
            if (client == null)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "No client found");
            }
            if (client.RequireClientSecret)
            {
                await _secretsValidator.ValidateAsync(clientSecret, client.ClientSecrets);
            }
            #endregion

            #region Validate Scopes
            var form = await context.Request.ReadFormAsNameValueCollectionAsync();
            var scope = form[OpenIdConnectParameterNames.Scope];
            if (string.IsNullOrWhiteSpace(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            await _scopeValidator.ValidateAsync(client.AllowedScopes, scopes);
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidGrant, "Grant Type is missing");
            }
            await _grantTypeValidator.ValidateAsync(grantType, client.AllowedGrantTypes);
            #endregion

            #region Validate Resources
            var resources = await _resources.FindByScopeAsync(scopes);
            await _resourceValidator.ValidateAsync(resources, scopes);
            #endregion

            #region Validate Grant
            var validatedRequest = new TokenValidatedRequest(
                client: client,
                clientSecret: clientSecret,
                options: _options,
                scopes: scopes,
                resources: resources,
                grantType: grantType,
                raw: form);
            var grantResult = await ValidateGrantAsync(context, validatedRequest);
            #endregion

            #region Generator Response
            var response = await _generator.ProcessAsync(new ValidatedTokenRequest(grantResult.Subject, client, resources)
            {
                Scopes = scopes,
                GrantType = grantType,
            });
            return TokenResult(response);
            #endregion
        }

        private async Task<GrantValidationResult> ValidateGrantAsync(HttpContext context, TokenValidatedRequest request)
        {
            //验证刷新令牌
            if (GrantTypes.RefreshToken.Equals(request.GrantType))
            {
                var refreshToken = request.Raw[OpenIdConnectParameterNames.RefreshToken];
                if (refreshToken == null)
                {
                    throw new InvalidRequestException("RefreshToken is missing");
                }
                if (refreshToken.Length > _options.InputLengthRestrictions.RefreshToken)
                {
                    throw new InvalidRequestException("RefreshToken too long");
                }
                var grantContext = new RefreshTokenGrantValidationContext(refreshToken, request);
                var grantValidator = context.RequestServices.GetRequiredService<IRefreshTokenGrantValidator>();
                return await grantValidator.ValidateAsync(grantContext);
            }
            //验证客户端凭据授权
            else if (GrantTypes.ClientCredentials.Equals(request.GrantType))
            {
                var grantContext = new ClientCredentialsGrantValidationContext(request);
                var grantValidator = context.RequestServices
                    .GetRequiredService<IClientCredentialsGrantValidator>();
                return await grantValidator.ValidateAsync(grantContext);
            }
            //验证资源所有者密码授权
            else if (GrantTypes.Password.Equals(request.GrantType))
            {
                var username = request.Raw[OpenIdConnectParameterNames.Username];
                var password = request.Raw[OpenIdConnectParameterNames.Password];
                if (string.IsNullOrEmpty(username))
                {
                    throw new InvalidRequestException("Username is missing");
                }
                if (username.Length > _options.InputLengthRestrictions.UserName)
                {
                    throw new InvalidRequestException("Username too long");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new InvalidRequestException("Password is missing");
                }
                if (password.Length > _options.InputLengthRestrictions.Password)
                {
                    throw new InvalidRequestException("Password too long");
                }
                var grantContext = new ResourceOwnerPasswordGrantValidationContext(
                    request: request,
                    username: username,
                    password: password);
                var grantValidator = context.RequestServices
                   .GetRequiredService<IResourceOwnerPasswordGrantValidator>();
                return await grantValidator.ValidateAsync(grantContext);
            }
            //验证自定义授权
            else
            {
                var grantContext = new ExtensionGrantValidationContext(request);
                var grantValidator = context.RequestServices
                    .GetRequiredService<IExtensionGrantsListValidator>();
                return await grantValidator.ValidateAsync(grantContext);
            }

        }
    }
}
