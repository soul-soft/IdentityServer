using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

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
            var authenticateResult = await context.AuthenticateAsync();
            if (authenticateResult == null || !authenticateResult.Succeeded)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidRequest, "authentication failed");
            }
            var client = authenticateResult.Properties.GetParameter<IClient>("client");
            if (client == null)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidRequest, "Client deleted or disabled");
            }
            var subject = authenticateResult.Principal;
            var isActive = new IsActiveContext(
                client,
                authenticateResult.Principal,
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
            throw new NotImplementedException();
        }

    }
}
