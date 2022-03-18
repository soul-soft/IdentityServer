using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly IProfileService _profileService;
        private readonly ITokenResponseGenerator _generator;
        private readonly IResourceValidator _resourceValidator;
        private readonly IClientSecretValidator _clientSecretValidator;
        private readonly IAuthenticationService _authenticationService;

        public TokenEndpoint(
            IdentityServerOptions options,
            IProfileService profileService,
            ITokenResponseGenerator generator,
            IClientSecretValidator clientSecretValidator,
            IResourceValidator resourceValidator,
            IAuthenticationService authenticationService)
        {
            _options = options;
            _generator = generator;
            _profileService = profileService;
            _clientSecretValidator = clientSecretValidator;
            _resourceValidator = resourceValidator;
            _authenticationService = authenticationService;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
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
                return BadRequest(OpenIdConnectValidationErrors.UnauthorizedClient, "Grant type not allowed");
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
            var profileDataRequestContext = new ProfileDataRequestContext(
                ProfileDataCallers.TokenEndpoint,
                result.Subject,
                request.Client, request.Resources);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            var singInAuthenticationContext = new SingInAuthenticationContext(request.Client, request.Resources, request.GrantType, profiles);
            var subject = await _authenticationService.SingInAsync(singInAuthenticationContext);
            if (!string.IsNullOrEmpty(subject.GetSubjectId()))
            {
                var isActive = await _profileService.IsActiveAsync(new IsActiveContext(request.Client, subject));
                if (!isActive)
                {
                    throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, string.Format("User has been disabled:{0}", subject.GetSubjectId()));
                }
            }
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
