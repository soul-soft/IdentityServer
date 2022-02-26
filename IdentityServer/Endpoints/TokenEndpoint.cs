using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IScopeParser _scopeParser;
        private readonly ITokenGenerator _generator;
        private readonly IClaimsService _claimsService;
        private readonly IdentityServerOptions _options;
        private readonly IScopeValidator _scopeValidator;
        private readonly IClaimsValidator _claimsValidator;
        private readonly ClientSecretParserCollection _secretParsers;
        private readonly IGrantTypeValidator _grantTypeValidator;
        private readonly SecretValidatorCollection _secretValidators;

        public TokenEndpoint(
            IClientStore clients,
            IScopeParser scopeParser,
            ITokenGenerator generator,
            IClaimsService claimsService,
            IdentityServerOptions options,
            IScopeValidator scopeValidator,
            IClaimsValidator claimsValidator,
            ClientSecretParserCollection secretParsers,
            IGrantTypeValidator grantTypeValidator,
            SecretValidatorCollection secretValidators)
        {
            _clients = clients;
            _options = options;
            _generator = generator;
            _scopeParser = scopeParser;
            _secretParsers = secretParsers;
            _scopeValidator = scopeValidator;
            _claimsService = claimsService;
            _claimsValidator = claimsValidator;
            _secretValidators = secretValidators;
            _grantTypeValidator = grantTypeValidator;
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
                return BadRequest();
            }
            #endregion

            #region Validate ClientSecret
            var clientSecret = await _secretParsers.ParseAsync(context);
            var client = await _clients.GetAsync(clientSecret.Id);
            if (client == null)
            {
                throw new InvalidClientException("Invalid client credentials");
            }
            if (client.RequireClientSecret)
            {
                await _secretValidators.ValidateAsync(clientSecret, client.ClientSecrets);
            }
            #endregion

            #region Validate Scopes
            var form = await context.Request.ReadFormAsNameValueCollectionAsync();
            var scope = form[OpenIdConnectParameterNames.Scope];
            if (string.IsNullOrEmpty(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = await _scopeParser.ParseAsync(scope);
            var resources = await _scopeValidator.ValidateAsync(client, scopes);
            #endregion

            #region Validate GrantType
            var grantType = form[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                throw new InvalidGrantException("Grant Type is missing");
            }
            await _grantTypeValidator.ValidateAsync(grantType, client.AllowedGrantTypes);
            #endregion

            #region Validate Grant
            var grantValidationRequest = new GrantRequest(
                client: client,
                clientSecret: clientSecret,
                options: _options,
                scopes: scopes,
                resources: resources,
                grantType: grantType,
                raw: form);
            var grantValidationResult = await ValidateGrantAsync(context, grantValidationRequest);
            #endregion

            #region Validate Claims
            var subject = await _claimsService.CreateSubjectAsync(grantValidationRequest, grantValidationResult);
            await _claimsValidator.ValidateAsync(subject, resources);
            #endregion

            #region Generator Response
            var response = await _generator.ProcessAsync(new ValidatedTokenRequest(subject, client, resources)
            {
                Scopes = scopes,
                GrantType = grantType,
            });
            return TokenEndpointResult(response);
            #endregion
        }

        private async Task<GrantValidationResult> ValidateGrantAsync(HttpContext context, GrantRequest request)
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
                   .GetRequiredService<IPasswordGrantValidator>();
                return await grantValidator.ValidateAsync(grantContext);
            }
            //验证自定义授权
            else
            {
                var grantContext = new ExtensionGrantValidationContext(request);
                var grantValidator = context.RequestServices
                    .GetRequiredService<ExtensionGrantValidatorCollection>();
                return await grantValidator.ValidateAsync(grantContext);
            }

        }
    }
}
