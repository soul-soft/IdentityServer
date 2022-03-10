using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly ITokenResponseGenerator _generator;
        private readonly IdentityServerOptions _options;
        private readonly IScopeValidator _scopeValidator;
        private readonly SecretValidatorCollection _secretValidators;
        private readonly ClientCredentialsParserCollection _clientCredentialsParsers;

        public TokenEndpoint(
            IClientStore clients,
            ITokenResponseGenerator generator,
            IdentityServerOptions options,
            IScopeValidator scopeValidator,
            SecretValidatorCollection secretValidators,
            ClientCredentialsParserCollection clientCredentialsParsers)
        {
            _clients = clients;
            _options = options;
            _generator = generator;
            _scopeValidator = scopeValidator;
            _secretValidators = secretValidators;
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
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient, "Invalid client credentials");
            }
            if (client.RequireClientSecret)
            {
                await _secretValidators.ValidateAsync(clientCredentials, client.ClientSecrets);
            }
            #endregion

            #region Validate Scopes
            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();
            var scope = form[OpenIdConnectParameterNames.Scope] ?? string.Empty;
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope, "Scope is too long");
            }
            if (string.IsNullOrEmpty(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = scope.Split(",")
                .Where(a => !string.IsNullOrWhiteSpace(a));
            var resources = await _scopeValidator.ValidateAsync(client.AllowedScopes, scopes);
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectTokenErrors.UnsupportedGrantType, "Grant type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return BadRequest(OpenIdConnectTokenErrors.UnsupportedGrantType, "Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return BadRequest(OpenIdConnectTokenErrors.UnsupportedGrantType, "Grant type not allowed");
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
                    throw new InvalidRequestException("RefreshToken is missing");
                }
                if (refreshToken.Length > _options.InputLengthRestrictions.RefreshToken)
                {
                    throw new InvalidRequestException("RefreshToken too long");
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
