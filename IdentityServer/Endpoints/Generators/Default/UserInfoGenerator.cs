namespace IdentityServer.Endpoints
{
    internal class UserInfoGenerator : IUserInfoGenerator
    {
        private readonly IProfileService _profileService;

        public UserInfoGenerator(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<UserInfoResponse> ProcessAsync(UserInfoGeneratorRequest request)
        {
            var profileDataRequestContext = new ProfileDataRequestContext(ProfileDataCallers.UserInfoEndpoint, request.Client, request.Resources.ClaimTypes);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            return new UserInfoResponse(profiles);
        }
    }
}
