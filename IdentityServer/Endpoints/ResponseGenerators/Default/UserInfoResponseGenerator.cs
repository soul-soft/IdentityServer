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

        public async Task<UserInfoGeneratorResponse> ProcessAsync(ClaimsPrincipal subject, Client client, Resources resources)
        {
            var profileDataRequestContext = new ProfileDataRequest(subject, client, resources);
            var claims = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            return new UserInfoGeneratorResponse(claims.ToClaimsDictionary());
        }
    }
}
