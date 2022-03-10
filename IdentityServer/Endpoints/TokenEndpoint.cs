using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resourceStore;
        private readonly IdentityServerOptions _options;
        private readonly ITokenResponseGenerator _generator;
        private readonly IResourceValidator _resourceValidator;
        private readonly SecretValidatorCollection _secretValidators;
        private readonly ClientCredentialsParserCollection _clientCredentialsParsers;

        public TokenEndpoint(
            IClientStore clients,
            IResourceStore resourceStore,
            IdentityServerOptions options,
            ITokenResponseGenerator generator,
            IResourceValidator resourceValidator,
            SecretValidatorCollection secretValidators,
            ClientCredentialsParserCollection clientCredentialsParsers)
        {
            _clients = clients;
            _options = options;
            _generator = generator;
            _resourceStore = resourceStore;
            _secretValidators = secretValidators;
            _resourceValidator = resourceValidator;
            _clientCredentialsParsers = clientCredentialsParsers;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            #region ValidateRequest
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Validate ClientSecret
            var clientCredentials = await _clientCredentialsParsers.ParseAsync(context);
            var client = await _clients.FindByClientIdAsync(clientCredentials.ClientId);
            if (client == null)
            {
                return BadRequest(OpenIdConnectErrors.InvalidClient, "Invalid client credentials");
            }
            if (client.RequireClientSecret)
            {
                await _secretValidators.ValidateAsync(clientCredentials, client.ClientSecrets);
            }
            #endregion

            #region Validate Resources
            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();
            var scope = form[OpenIdConnectParameterNames.Scope] ?? string.Empty;
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                return BadRequest(OpenIdConnectErrors.InvalidScope, "Scope is too long");
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrWhiteSpace(a));
            if (scopes.Any())
            {
                foreach (var item in scopes)
                {
                    if (!client.AllowedScopes.Contains(item))
                    {
                        return BadRequest(OpenIdConnectErrors.InvalidScope, $"Scope '{item}' not allowed");
                    }
                }
            }
            else
            {
                scopes = client.AllowedScopes;
            }
            if (!scopes.Any())
            {
                return BadRequest(OpenIdConnectErrors.InvalidScope, "No allowed scopes configured for client");
            }
            var resources = await _resourceStore.FindResourceByScopesAsync(scopes);
            await _resourceValidator.ValidateAsync(scopes, resources);
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectErrors.UnsupportedGrantType, "Grant type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return BadRequest(OpenIdConnectErrors.UnsupportedGrantType, "Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return BadRequest(OpenIdConnectErrors.UnsupportedGrantType, "Grant type not allowed");
            }
            #endregion

            #region Validation Token
            var validationTokenRequest = new TokenValidationRequest(
                client: client,
                clientSecret: clientCredentials,
                options: _options,
                scopes: scopes,
                resources: resources,
                grantType: grantType,
                raw: form);
            await RunTokenValidationAsync(context, validationTokenRequest);
            #endregion

            #region Generator Response
            var validatedTokenRequest = new TokenValidatedRequest(grantType, client, resources, _options);
            var response = await _generator.ProcessAsync(validatedTokenRequest);
            return TokenEndpointResult(response);
            #endregion
        }

        private async Task RunTokenValidationAsync(HttpContext context, TokenValidationRequest request)
        {
            //验证刷新令牌
            if (GrantTypes.RefreshToken.Equals(request.GrantType))
            {
                var refreshToken = request.Raw[OpenIdConnectParameterNames.RefreshToken];
                if (refreshToken == null)
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "RefreshToken is missing");
                }
                if (refreshToken.Length > _options.InputLengthRestrictions.RefreshToken)
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "RefreshToken too long");
                }
                var grantContext = new RefreshTokenGrantValidationRequest(refreshToken, request);
                var grantValidator = context.RequestServices.GetRequiredService<IRefreshTokenGrantValidator>();
                await grantValidator.ValidateAsync(grantContext);
            }
            //验证客户端凭据授权
            else if (GrantTypes.ClientCredentials.Equals(request.GrantType))
            {
                var grantContext = new ClientGrantValidationRequest(request);
                var grantValidator = context.RequestServices.GetRequiredService<IClientCredentialsGrantValidator>();
                await grantValidator.ValidateAsync(grantContext);
            }
            //验证资源所有者密码授权
            else if (GrantTypes.Password.Equals(request.GrantType))
            {
                var username = request.Raw[OpenIdConnectParameterNames.Username];
                var password = request.Raw[OpenIdConnectParameterNames.Password];
                if (string.IsNullOrEmpty(username))
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Username is missing");
                }
                if (username.Length > _options.InputLengthRestrictions.UserName)
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Username too long");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Password is missing");
                }
                if (password.Length > _options.InputLengthRestrictions.Password)
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Password too long");
                }
                var grantContext = new PasswordGrantValidationRequest(
                    request: request,
                    username: username,
                    password: password);
                var grantValidator = context.RequestServices.GetRequiredService<IPasswordGrantValidator>();
                await grantValidator.ValidateAsync(grantContext);
            }
            //验证自定义授权
            else
            {
                var grantContext = new ExtensionGrantValidationRequest(request);
                var grantValidator = context.RequestServices.GetRequiredService<ExtensionGrantValidatorCollection>();
                await grantValidator.ValidateAsync(grantContext);
            }
        }
    }
}
