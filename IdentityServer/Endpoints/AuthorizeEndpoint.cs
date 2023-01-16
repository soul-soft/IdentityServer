using IdentityServer.Models;
using IdentityServer.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeEndpoint : EndpointBase
    {
        private readonly ITokenParser _tokenParser;
        private readonly ITokenValidator _tokenValidator;
        private readonly IProfileService _profileService;
        private readonly IdentityServerOptions _options;
        private readonly IResourceValidator _resourceValidator;
        private readonly IAuthorizeResponseGenerator _generator;
        private readonly IClientSecretValidator _clientSecretValidator;

        public AuthorizeEndpoint(
            ITokenParser tokenParser,
            ITokenValidator tokenValidator,
            IProfileService profileService,
            IdentityServerOptions options,
            IResourceValidator resourceValidator,
            IAuthorizeResponseGenerator generator,
            IClientSecretValidator clientSecretValidator)
        {
            _options = options;
            _generator = generator;
            _tokenParser = tokenParser;
            _profileService = profileService;
            _tokenValidator = tokenValidator;
            _resourceValidator = resourceValidator;
            _clientSecretValidator = clientSecretValidator;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Validate Request
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            var query = context.Request.Query.AsNameValueCollection();
            #endregion

            #region state
            var state = query[OpenIdConnectParameterNames.State];
            if (string.IsNullOrEmpty(state))
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "State type is missing");
            }
            #endregion

            #region RedirectUri
            var redirectUri = query[OpenIdConnectParameterNames.RedirectUri];
            if (string.IsNullOrEmpty(redirectUri))
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "RedirectUri type is missing");
            }
            #endregion

            #region ResponseType
            var responseType = query[OpenIdConnectParameterNames.ResponseType];
            if (string.IsNullOrEmpty(responseType))
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "ResponseType type is missing");
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

            #region Token
            var token = await _tokenParser.ParserAsync(context);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "Token is missing");
            }
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (tokenValidationResult.IsError)
            {
                return Unauthorized(tokenValidationResult.Error, tokenValidationResult.ErrorDescription);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(tokenValidationResult.Claims, "UserInfo"));
            if (!subject.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                return Unauthorized(OpenIdConnectValidationErrors.InsufficientScope, $"Token contains no sub claim");
            }
            var isActive = await _profileService.IsActiveAsync(new IsActiveRequest(
                ProfileIsActiveCallers.AuthorizeEndpoint,
                tokenValidationResult.Client,
                subject));
            if (!isActive)
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, $"User marked as not active: {subject.GetSubjectId()}");
            }
            #endregion

            #region Generator
            var request = new AuthorizeGeneratorRequest(redirectUri, responseType, client, resources, subject, _options);
            var response = await _generator.ProcessAsync(request);
            #endregion

            return AuthorizeEndpointResult(response);
        }
    }
}
