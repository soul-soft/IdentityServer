using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClaimService _claimService;
        private readonly IdentityServerOptions _options;
        private readonly IProfileService _profileService;
        private readonly ITokenResponseGenerator _generator;
        private readonly IResourceValidator _resourceValidator;
        private readonly IClientSecretValidator _clientSecretValidator;

        public TokenEndpoint(
            IClaimService claimService,
            IdentityServerOptions options,
            IProfileService profileService,
            ITokenResponseGenerator generator,
            IClientSecretValidator clientSecretValidator,
            IResourceValidator resourceValidator)
        {
            _options = options;
            _generator = generator;
            _claimService = claimService;
            _profileService = profileService;
            _clientSecretValidator = clientSecretValidator;
            _resourceValidator = resourceValidator;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Validate Request
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Validate Client
            var client = await _clientSecretValidator.ValidateAsync(context);
            #endregion

            #region Validate Resources
            var parameters = await context.Request.ReadFormAsync();
            var body = parameters.AsNameValueCollection();
            var scope = body[OpenIdConnectParameterNames.Scope] ?? string.Empty;
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                return BadRequest(ValidationErrors.InvalidScope, "Scope is too long");
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrWhiteSpace(a));
            var resources = await _resourceValidator.ValidateAsync(client, scopes);
            #endregion

            #region Validate GrantType
            var grantType = body[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Grant type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return BadRequest(ValidationErrors.UnauthorizedClient, $"The client does not allow {grantType}");
            }
            #endregion

            #region Validate Grant
            var subject = await RunGrantValidationAsync(context, new GrantValidationRequest(client, grantType, resources, body, _options));
            #endregion

            #region Response Generator
            var response = await _generator.ProcessAsync(new TokenGeneratorRequest(grantType, subject, client, resources, _options));
            return TokenEndpointResult(response);
            #endregion
        }

        #region Validate Grant
        private async Task<ClaimsPrincipal> RunGrantValidationAsync(HttpContext context, GrantValidationRequest request)
        {
            //验证刷新令牌
            GrantValidationResult result;
            if (GrantTypes.RefreshToken.Equals(request.GrantType))
            {
                result = await ValidateRefreshTokenRequestAsync(context, request);
            }
            //验证客户端凭据授权
            else if (GrantTypes.ClientCredentials.Equals(request.GrantType))
            {
                result = await ValidateClientCredentialsRequestAsync(context, request);
            }
            //授权码凭据授权
            else if (GrantTypes.AuthorizationCode.Equals(request.GrantType))
            {
                result = await ValidateAuthorizeCodeRequestAsync(context, request);
            }
            //验证资源所有者密码授权
            else if (GrantTypes.Password.Equals(request.GrantType))
            {
                result = await ValidateResourceOwnerCredentialRequestAsync(context, request);
            }
            //验证自定义授权
            else
            {
                result = await ValidateExtensionGrantRequestAsync(context, request);
            }
            //验证是否启用
            var isActiveRequest = new IsActiveRequest(ProfileIsActiveCallers.TokenEndpoint, request.Client, result.Subject);
            var isActive = await _profileService.IsActiveAsync(isActiveRequest);
            if (!isActive)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, string.Format("User has been disabled:{0}", result.Subject.GetSubjectId()));
            }
            //issue claims
            var accessTokenClaimsRequest = new ProfileClaimsRequest(result.Subject, request.Client, request.Resources);
            var subject = await _claimService.GetAccessTokenClaimsAsync(request.GrantType, accessTokenClaimsRequest);
            return subject;
        }
        #endregion

        #region ClientCredentialsRequest
        private static async Task<GrantValidationResult> ValidateClientCredentialsRequestAsync(HttpContext context, GrantValidationRequest request)
        {
            var validator = context.RequestServices.GetRequiredService<IClientCredentialsRequestValidator>();
            var grantContext = new ClientCredentialsValidationRequest(request);
            return await validator.ValidateAsync(grantContext);
        }
        #endregion

        #region AuthorizeCodeRequest
        private static async Task<GrantValidationResult> ValidateAuthorizeCodeRequestAsync(HttpContext context, GrantValidationRequest request)
        {
            var code = request.Form[OpenIdConnectParameterNames.Code];
            if (string.IsNullOrEmpty(code))
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "Code is missing");
            }
            var grantContext = new AuthorizeCodeValidationRequest(code, request);
            var validator = context.RequestServices.GetRequiredService<IAuthorizeCodeRequestValidator>();
            return await validator.ValidateAsync(grantContext);
        }
        #endregion

        #region ResourceOwnerCredentialRequest
        private async Task<GrantValidationResult> ValidateResourceOwnerCredentialRequestAsync(HttpContext context, GrantValidationRequest request)
        {
            var username = request.Form[OpenIdConnectParameterNames.Username];
            var password = request.Form[OpenIdConnectParameterNames.Password];
            if (string.IsNullOrEmpty(username))
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "Username is missing");
            }
            if (username.Length > _options.InputLengthRestrictions.UserName)
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "Username too long");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "Password is missing");
            }
            if (password.Length > _options.InputLengthRestrictions.Password)
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "Password too long");
            }
            var validation = new ResourceOwnerCredentialValidationRequest(username, password, request);
            var validator = context.RequestServices.GetRequiredService<IResourceOwnerCredentialRequestValidator>();
            return await validator.ValidateAsync(validation);
        }
        #endregion

        #region RefreshTokenRequest
        private async Task<GrantValidationResult> ValidateRefreshTokenRequestAsync(HttpContext context, GrantValidationRequest request)
        {
            var refreshToken = request.Form[OpenIdConnectParameterNames.RefreshToken];
            if (refreshToken == null)
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "RefreshToken is missing");
            }
            if (refreshToken.Length > _options.InputLengthRestrictions.RefreshToken)
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "RefreshToken too long");
            }
            var validator = context.RequestServices.GetRequiredService<IRefreshTokenRequestValidator>();
            return await validator.ValidateAsync(new RefreshTokenValidationRequest(refreshToken, request));
        }
        #endregion

        #region ExtensionGrantRequest
        private static async Task<GrantValidationResult> ValidateExtensionGrantRequestAsync(HttpContext context, GrantValidationRequest request)
        {
            var grantContext = new ExtensionGrantValidationRequest(request);
            var validator = context.RequestServices.GetRequiredService<IExtensionGrantListValidator>();
            var result = await validator.ValidateAsync(grantContext);
            return result;
        }
        #endregion
    }
}
