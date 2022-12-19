using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly IClaimService _claimService;
        private readonly IProfileService _profileService;
        private readonly ITokenResponseGenerator _generator;
        private readonly IResourceValidator _resourceValidator;
        private readonly IClientSecretValidator _clientSecretValidator;

        public TokenEndpoint(
            IClaimService singInService,
            IdentityServerOptions options,
            IProfileService profileService,
            ITokenResponseGenerator generator,
            IClientSecretValidator clientSecretValidator,
            IResourceValidator resourceValidator)
        {
            _options = options;
            _generator = generator;
            _claimService = singInService;
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
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Validate Client
            var client = await _clientSecretValidator.ValidateAsync(context);
            #endregion

            #region Validate Resources
            var from = await context.Request.ReadFormAsync();
            var body = from.AsNameValueCollection();
            var scope = body[OpenIdConnectParameterNames.Scope] ?? string.Empty;
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidScope, "Scope is too long");
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrWhiteSpace(a));
            var resources = await _resourceValidator.ValidateAsync(client, scopes);
            #endregion

            #region Validate GrantType
            var grantType = body[OpenIdConnectParameterNames.GrantType];
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "Grant type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return BadRequest(OpenIdConnectValidationErrors.UnauthorizedClient, $"The client does not allow {grantType}");
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
                var validator = context.RequestServices.GetRequiredService<IRefreshTokenRequestValidator>();
                result = await ValidateRefreshTokenRequestAsync(validator, request);
            }
            //验证客户端凭据授权
            else if (GrantTypes.ClientCredentials.Equals(request.GrantType))
            {
                var validator = context.RequestServices.GetRequiredService<IClientCredentialsRequestValidator>();
                result = await ValidateClientCredentialsRequestAsync(validator, request);
            }
            //验证资源所有者密码授权
            else if (GrantTypes.Password.Equals(request.GrantType))
            {
                var validator = context.RequestServices.GetRequiredService<IResourceOwnerCredentialRequestValidator>();
                result = await ValidateResourceOwnerCredentialRequestAsync(validator, request);
            }
            //验证自定义授权
            else
            {
                var validator = context.RequestServices.GetRequiredService<IExtensionGrantListValidator>();
                result = await ValidateExtensionGrantRequestAsync(validator, request);
            }
            //验证用户是否启用
            if (!string.IsNullOrEmpty(result.Subject.GetSubjectId()))
            {
                var isActive = await _profileService.IsActiveAsync(new IsActiveRequest(
                    ProfileIsActiveCallers.TokenEndpoint,
                    request.Client,
                    result.Subject));
                if (!isActive)
                {
                    throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, string.Format("User has been disabled:{0}", result.Subject.GetSubjectId()));
                }
            }
            //sing claims
            var singInAuthenticationContext = new SingInAuthenticationContext(request.Client, result.Subject, request.Resources, request.GrantType);
            var subject = await _claimService.SignClaimsInAsync(singInAuthenticationContext);
            return subject;
        }
        #endregion

        #region ClientCredentialsRequest
        private static async Task<GrantValidationResult> ValidateClientCredentialsRequestAsync(IClientCredentialsRequestValidator validator, GrantValidationRequest request)
        {
            var grantContext = new ClientCredentialsValidationRequest(request);
            return await validator.ValidateAsync(grantContext);
        }
        #endregion

        #region ResourceOwnerCredentialRequest
        private async Task<GrantValidationResult> ValidateResourceOwnerCredentialRequestAsync(IResourceOwnerCredentialRequestValidator validator, GrantValidationRequest request)
        {
            var username = request.Body[OpenIdConnectParameterNames.Username];
            var password = request.Body[OpenIdConnectParameterNames.Password];
            if (string.IsNullOrEmpty(username))
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidRequest, "Username is missing");
            }
            if (username.Length > _options.InputLengthRestrictions.UserName)
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidRequest, "Username too long");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidRequest, "Password is missing");
            }
            if (password.Length > _options.InputLengthRestrictions.Password)
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidRequest, "Password too long");
            }
            var validation = new ResourceOwnerCredentialValidationRequest(username, password, request);
            return await validator.ValidateAsync(validation);
        }
        #endregion

        #region RefreshTokenRequest
        private async Task<GrantValidationResult> ValidateRefreshTokenRequestAsync(IRefreshTokenRequestValidator validator, GrantValidationRequest request)
        {
            var refreshToken = request.Body[OpenIdConnectParameterNames.RefreshToken];
            if (refreshToken == null)
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidRequest, "RefreshToken is missing");
            }
            if (refreshToken.Length > _options.InputLengthRestrictions.RefreshToken)
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidRequest, "RefreshToken too long");
            }
            return await validator.ValidateAsync(new RefreshTokenValidationRequest(refreshToken, request));
        }
        #endregion

        #region ExtensionGrantRequest
        private static async Task<GrantValidationResult> ValidateExtensionGrantRequestAsync(IExtensionGrantListValidator validator, GrantValidationRequest request)
        {
            var grantContext = new ExtensionGrantValidationRequest(request);
            return await validator.ValidateAsync(grantContext);
        }
        #endregion
    }
}
