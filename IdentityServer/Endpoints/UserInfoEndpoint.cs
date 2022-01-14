using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {

        private readonly ITokenValidator _tokenValidator;
        private readonly IProfileService _profileService;
        private readonly IBearerTokenUsageParser _bearerTokenUsageParser;

        public UserInfoEndpoint(
            ITokenValidator tokenValidator,
            IProfileService profileService,
            IBearerTokenUsageParser bearerTokenUsageParser)
        {
            _tokenValidator = tokenValidator;
            _profileService = profileService;
            _bearerTokenUsageParser = bearerTokenUsageParser;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var token = await _bearerTokenUsageParser.ParserAsync(context);
            var validationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (validationResult.IsError)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidToken, validationResult.Description);
            }
            var client = validationResult.Client;
            var subject = validationResult.Subject;
            var isActive = new IsActiveContext(
                client,
                subject,
                ProfileIsActiveCaller.UserInfoRequestValidation);
            await _profileService.IsActiveAsync(isActive);
            if (!isActive.IsActive)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidRequest, string.Format("User is not active: {sub}", subject.GetSubjectId()));
            }
            //var profileDataCotnext = new ProfileDataRequestContext(
            //    client,
            //    subject,
            //    ProfileDataCaller.UserInfoEndpoint);
            //await _profileService.GetProfileDataAsync(profileDataCotnext);
            return Unauthorized(OpenIdConnectTokenErrors.InvalidToken, validationResult.Description);
        }

    }
}
