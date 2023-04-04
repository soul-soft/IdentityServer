using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly ITokenParser _tokenParser;
        private readonly IResourceStore _resourceStore;
        private readonly ITokenValidator _tokenValidator;
        private readonly IProfileService _profileService;
        private readonly IUserInfoResponseGenerator _generator;

        public UserInfoEndpoint(
            ITokenParser tokenParser,
            IResourceStore resourceStore,
            IProfileService profileService,
            ITokenValidator tokenValidator,
            IUserInfoResponseGenerator generator)
        {
            _generator = generator;
            _tokenParser = tokenParser;
            _resourceStore = resourceStore;
            _tokenValidator = tokenValidator;
            _profileService = profileService;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            var token = await _tokenParser.ParserAsync(context);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "Token is missing");
            }
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (tokenValidationResult.IsError)
            {
                return Unauthorized(tokenValidationResult.Error, tokenValidationResult.ErrorDescription);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(tokenValidationResult.Claims, "UserInfo"));
            if (!subject.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                return Unauthorized(ValidationErrors.InsufficientScope, $"Token contains no sub claim");
            }
            var isActive = await _profileService.IsActiveAsync(new IsActiveRequest(
                ProfileIsActiveCallers.UserInfoEndpoint,
                tokenValidationResult.Client,
                subject));
            if (!isActive)
            {
                return BadRequest(ValidationErrors.InvalidRequest, $"User marked as not active: {subject.GetSubjectId()}");
            }
            var scopes = subject.FindAll(JwtClaimTypes.Scope).Select(s => s.Value);
            var client = tokenValidationResult.Client;
            var resources = await _resourceStore.FindResourcesByScopesAsync(scopes);
            var response = await _generator.ProcessAsync(subject, client, resources);
            return new UserInfoEndpointResult(response);
        }
    }
}
