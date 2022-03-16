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
        private readonly ISecretListParser _secretsParser;
        private readonly IResourceValidator _resourceValidator;
        private readonly ISecretListValidator  _secretsValidator;

        public TokenEndpoint(
            IClientStore clients,
            IResourceStore resourceStore,
            IdentityServerOptions options,
            ISecretListParser secretsParser,
            ITokenResponseGenerator generator,
            IResourceValidator resourceValidator,
            ISecretListValidator secretsValidator)
        {
            _clients = clients;
            _options = options;
            _generator = generator;
            _resourceStore = resourceStore;
            _secretsParser = secretsParser;
            _secretsValidator = secretsValidator;
            _resourceValidator = resourceValidator;
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
            var parsedSecret = await _secretsParser.ParseAsync(context);
            var client = await _clients.FindByClientIdAsync(parsedSecret.ClientId);
            if (client == null)
            {
                return BadRequest(OpenIdConnectErrors.InvalidClient, "Invalid client credentials");
            }
            if (client.RequireClientSecret)
            {
                await _secretsValidator.ValidateAsync(parsedSecret, client.ClientSecrets);
            }
            #endregion

            #region Validate Resources
            var body = await context.Request.ReadFormAsync();
            var form = body.AsNameValueCollection();
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
            var resources = await _resourceStore.FindResourcesByScopesAsync(scopes);
            await _resourceValidator.ValidateAsync(scopes, resources);
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Grant type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return BadRequest(OpenIdConnectErrors.UnauthorizedClient, "Grant type not allowed");
            }
            #endregion

            #region Validate GrantRequest
            var validationTokenRequest = new TokenRequestValidation(
                client: client,
                clientSecret: parsedSecret,
                options: _options,
                scopes: scopes,
                resources: resources,
                grantType: grantType,
                raw: form);
            await RunValidationAsync(context, validationTokenRequest);
            #endregion

            #region Generator Response
            var validatedTokenRequest = new TokenValidatedRequest(grantType, client, resources, _options);
            var response = await _generator.ProcessAsync(validatedTokenRequest);
            return TokenEndpointResult(response);
            #endregion
        }

        #region Validate GrantRequest
        private async Task RunValidationAsync(HttpContext context, TokenRequestValidation request)
        {
            //验证刷新令牌
            if (GrantTypes.RefreshToken.Equals(request.GrantType))
            {
                var validator = context.RequestServices.GetRequiredService<IRefreshTokenRequestValidator>();
                await ValidateRefreshTokenRequestAsync(validator, request);
            }
            //验证客户端凭据授权
            else if (GrantTypes.ClientCredentials.Equals(request.GrantType))
            {
                var validator = context.RequestServices.GetRequiredService<IClientCredentialsRequestValidator>();
                await ValidateClientCredentialsRequestAsync(validator, request);
            }
            //验证资源所有者密码授权
            else if (GrantTypes.Password.Equals(request.GrantType))
            {
                var validator = context.RequestServices.GetRequiredService<IResourceOwnerCredentialRequestValidator>();
                await ValidateResourceOwnerCredentialRequestAsync(validator, request);
            }
            //验证自定义授权
            else
            {
                var validator = context.RequestServices.GetRequiredService<IExtensionGrantListValidator>();
                await ValidateExtensionGrantRequestAsync(validator, request);
            }
        }
        #endregion

        #region ClientCredentialsRequest
        private static async Task ValidateClientCredentialsRequestAsync(IClientCredentialsRequestValidator validator, TokenRequestValidation request)
        {
            var grantContext = new ClientCredentialsRequestValidation(request);
            await validator.ValidateAsync(grantContext);
        }
        #endregion

        #region ResourceOwnerCredentialRequest
        private async Task ValidateResourceOwnerCredentialRequestAsync(IResourceOwnerCredentialRequestValidator validator, TokenRequestValidation request)
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
            var validation = new ResourceOwnerCredentialRequestValidation(request, username, password);
            await validator.ValidateAsync(validation);
        }
        #endregion

        #region RefreshTokenRequest
        private async Task ValidateRefreshTokenRequestAsync(IRefreshTokenRequestValidator validator, TokenRequestValidation request)
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
            await validator.ValidateAsync(new RefreshTokenRequestValidation(refreshToken, request));
        }
        #endregion

        #region ExtensionGrantRequest
        private static async Task ValidateExtensionGrantRequestAsync(IExtensionGrantListValidator validator, TokenRequestValidation request)
        {
            var grantContext = new ExtensionGrantRequestValidation(request);
            await validator.ValidateAsync(grantContext);
        }
        #endregion
    }
}
