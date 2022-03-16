using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoResponseGenerator : IUserInfoResponseGenerator
    {
        private readonly IProfileService _profileService;

        public UserInfoResponseGenerator(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<UserInfoResponse> ProcessAsync(ClaimsPrincipal subject, Client client, Resources resources)
        {
            var claimTypes = resources.ClaimTypes;
          
            var claims = await _profileService.GetProfileDataAsync(new ProfileDataRequestContext(
                ProfileDataCallers.UserInfoEndpoint,
                client,
                resources,
                claimTypes));
          
            return new UserInfoResponse(claims.ToClaimsDictionary());
        }
    }
}
