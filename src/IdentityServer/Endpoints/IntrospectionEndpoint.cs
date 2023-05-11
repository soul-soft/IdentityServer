using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class IntrospectionEndpoint : EndpointBase
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly IProfileService _profileService;
        private readonly IApiSecretValidator _apiSecretParsers;
        private readonly IIntrospectionResponseGenerator _generator;

        public IntrospectionEndpoint(
            ITokenValidator tokenValidator,
            IProfileService profileService,
            IApiSecretValidator apiSecretParsers,
            IIntrospectionResponseGenerator generator)
        {
            _generator = generator;
            _profileService = profileService;
            _tokenValidator = tokenValidator;
            _apiSecretParsers = apiSecretParsers;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Validate Method
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return StatusCode(System.Net.HttpStatusCode.UnsupportedMediaType);
            }
            #endregion

            #region Validate ApiSecret
            var apiResource = await _apiSecretParsers.ValidateAsync(context);
            #endregion

            #region Validate Token
            var body = await context.Request.ReadFormAsync();
            var form = body.AsNameValueCollection();
            var token = form.Get("token");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Token is missing");
            }
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            #endregion

            #region Validate Subject
            var subject = new ClaimsPrincipal(new ClaimsIdentity(tokenValidationResult.Claims, "Introspection"));
            if (!string.IsNullOrEmpty(subject.GetSubjectId()))
            {
                var isActive = await _profileService.IsActiveAsync(new IsActiveRequest(
                    ProfileIsActiveCallers.TokenEndpoint,
                    tokenValidationResult.Client,
                    subject));
                if (!isActive)
                {
                    throw new ValidationException(ValidationErrors.InvalidGrant, string.Format("User has been disabled:{0}", subject.GetSubjectId()));
                }
            }
            #endregion

            #region Response Generator
            var response = await _generator.ProcessAsync(new IntrospectionGeneratorRequest(
                !tokenValidationResult.IsError,
                tokenValidationResult.Client,
                subject,
                apiResource));
            #endregion

            return Json(response.Serialize());
        }
    }
}
