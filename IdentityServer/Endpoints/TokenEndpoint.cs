using IdentityServer.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            if (!context.Request.HasApplicationFormContentType())
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidRequest);
            }
            #endregion

            #region Get Secret
            var clientSecret = await _secretsParser.ParseAsync(context);
            if (clientSecret == null)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "No client credentials found");
            }
            #endregion

            #region Get Client
            var client = await _clients.FindClientByIdAsync(clientSecret.Id);
            if (client == null)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "No client found");
            }
            #endregion

            #region Validate Secret
            var validationResult = await _secretsValidator.ValidateAsync(clientSecret, client.ClientSecrets);
            if (validationResult.IsError)
            {
                return BadRequest(OpenIdConnectTokenErrors.UnauthorizedClient, validationResult.Description);
            }
            #endregion

            #region Get Scopes
            var form = await context.Request.ReadFormAsNameValueCollectionAsync();
            var scope = form[OpenIdConnectParameterNames.Scope];
            if (string.IsNullOrWhiteSpace(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            #endregion

            #region Validate Scopes
            validationResult = await _scopeValidator.Validate(client.AllowedScopes, scopes);
            if (validationResult.IsError)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope, validationResult.Description);
            }
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidGrant, "Grant Type is missing");
            }
            validationResult = await _grantTypeValidator.ValidateAsync(grantType, client.AllowedGrantTypes);
            if (validationResult.IsError)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidGrant, validationResult.Description);
            }
            #endregion

            #region Get Resources
            var resources = await _resources.FindResourcesByScopeAsync(scopes);
            #endregion

            #region Validate Resources
            validationResult = await _resourceValidator.ValidateAsync(resources, scopes);
            if (validationResult.IsError)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope, validationResult.Description);
            }
            #endregion

            #region Validate Grant
            var validatedRequest = new ValidatedRequest(
                client: client,
                clientSecret: clientSecret,
                options: _options,
                scopes: scopes,
                resources: resources,
                grantType: grantType,
                raw: form);
            var grantValidationResult = await ValidateGrantAsync(context, validatedRequest);
            if (grantValidationResult.IsError)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidGrant, grantValidationResult.Description);
            }
            #endregion

            #region Generator Response
            var response = await _generator.ProcessAsync(new TokenRequest(client, resources)
            {
                Scopes = scopes,
                GrantType = grantType,
                SubjectId = grantValidationResult.SubjectId,
                Claims = grantValidationResult.Claims,
            });
            return TokenResult(response);
            #endregion
        }

        private async Task<GrantValidationResult> ValidateGrantAsync(HttpContext context, ValidatedRequest request)
        {
            //验证刷新令牌
            if(GrantTypes.RefreshToken.Equals(request.GrantType))
            {
                var refreshToken = request.Raw[OpenIdConnectParameterNames.RefreshToken];
                if (refreshToken==null)
                {
                    return GrantValidationResult.Error("RefreshToken is missing");
                }
                if (refreshToken.Length>_options.InputLengthRestrictions.RefreshToken)
                {
                    return GrantValidationResult.Error("RefreshToken too long");
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
                    return GrantValidationResult.Error("Username is missing");
                }
                if (username.Length > _options.InputLengthRestrictions.UserName)
                {
                    return GrantValidationResult.Error("Username too long");
                }
                if (string.IsNullOrEmpty(password))
                {
                    return GrantValidationResult.Error("Password is missing");
                }
                if (password.Length > _options.InputLengthRestrictions.Password)
                {
                    return GrantValidationResult.Error("Password is missing");
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
