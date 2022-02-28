using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IScopeParser _scopeParser;
        private readonly ITokenGenerator _generator;
        private readonly IdentityServerOptions _options;
        private readonly IScopeValidator _scopeValidator;
        private readonly ClientSecretParserCollection _secretParsers;
        private readonly SecretValidatorCollection _secretValidators;

        public TokenEndpoint(
            IClientStore clients,
            IScopeParser scopeParser,
            ITokenGenerator generator,
            IdentityServerOptions options,
            IScopeValidator scopeValidator,
            ClientSecretParserCollection secretParsers,
            SecretValidatorCollection secretValidators)
        {
            _clients = clients;
            _options = options;
            _generator = generator;
            _scopeParser = scopeParser;
            _secretParsers = secretParsers;
            _scopeValidator = scopeValidator;
            _secretValidators = secretValidators;
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
                return BadRequest(ProtectedResourceErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Validate ClientSecret
            var clientCredentials = await _secretParsers.ParseAsync(context);
            var client = await _clients.FindByClientIdAsync(clientCredentials.ClientId);
            if (client == null)
            {
                throw new InvalidClientException("Invalid client credentials");
            }
            if (client.RequireClientSecret)
            {
                await _secretValidators.ValidateAsync(clientCredentials, client.ClientSecrets);
            }
            #endregion

            #region Validate Scopes
            var form = await context.Request.ReadFormAsNameValueCollectionAsync();
            var scope = form[OpenIdConnectParameterNames.Scope];
            if (string.IsNullOrEmpty(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = await _scopeParser.RequestScopeAsync(scope);
            var resources = await _scopeValidator.ValidateAsync(client.AllowedScopes, scopes);
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                throw new InvalidGrantException("Grant Type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                throw new InvalidGrantException("Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                throw new InvalidGrantException(string.Format("The client does not allow '{0}' authorization", grantType));
            }
            #endregion

            #region Validate Grant
            var grantValidationRequest = new TokenGrantValidationRequest(
                client: client,
                clientSecret: clientCredentials,
                options: _options,
                scopes: scopes,
                resources: resources,
                grantType: grantType,
                raw: form);
            var subject = await TokenValidateGrantAsync(context, grantValidationRequest);
            #endregion

            #region Generator Response
            var response = await _generator.ProcessAsync(new ValidatedTokenRequest(_options, subject, client, resources)
            {
                Scopes = scopes,
                GrantType = grantType,
            });
            return TokenEndpointResult(response);
            #endregion
        }

        private async Task<ClaimsPrincipal> TokenValidateGrantAsync(HttpContext context, TokenGrantValidationRequest request)
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
                var grantValidator = context.RequestServices.GetRequiredService<IClientGrantValidator>();
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
            var identity = new ClaimsIdentity(request.GrantType);
            return new ClaimsPrincipal(identity);
        }
    }
}
