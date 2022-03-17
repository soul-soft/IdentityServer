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
            var subject = await RunValidationAsync(context, new TokenRequestValidation(client, grantType, resources, body, _options));
            #endregion

            #region Response Generator
            var response = await _generator.ProcessAsync(new TokenRequest(grantType, subject, client, resources, _options));
            return TokenEndpointResult(response);
            #endregion
        }

        #region Validate Grant
        private async Task<ClaimsPrincipal> RunValidationAsync(HttpContext context, TokenRequestValidation request)
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
            var profileDataRequestContext = new ProfileDataRequestContext(ProfileDataCallers.TokenEndpoint, new ClaimsPrincipal(new ClaimsIdentity("Token")), request.Client, request.Resources);
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
        private static async Task ValidateClientCredentialsRequestAsync(IClientCredentialsRequestValidator validator, TokenRequestValidation request)
        {
            var grantContext = new ClientCredentialsValidation(request);
            await validator.ValidateAsync(grantContext);
        }
        #endregion

        #region ResourceOwnerCredentialRequest
        private async Task ValidateResourceOwnerCredentialRequestAsync(IResourceOwnerCredentialRequestValidator validator, TokenRequestValidation request)
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
            var validation = new ResourceOwnerCredentialValidation(request, username, password);
            await validator.ValidateAsync(validation);
        }
        #endregion

        #region RefreshTokenRequest
        private async Task ValidateRefreshTokenRequestAsync(IRefreshTokenRequestValidator validator, TokenRequestValidation request)
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
            await validator.ValidateAsync(new RefreshTokenValidation(refreshToken, request));
        }
        #endregion

        #region ExtensionGrantRequest
        private static async Task ValidateExtensionGrantRequestAsync(IExtensionGrantListValidator validator, TokenRequestValidation request)
        {
            var grantContext = new ExtensionGrantValidation(request);
            await validator.ValidateAsync(grantContext);
        }
        #endregion
    }
}
