namespace IdentityServer.Endpoints
{
    internal class UserInfoResponseGenerator : IUserInfoResponseGenerator
    {
        private readonly IProfileService _profileService;

        public UserInfoResponseGenerator(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<UserInfoResponse> ProcessAsync(UserInfoGeneratorRequest request)
        {
            var profileDataRequestContext = new ProfileDataRequestContext(
                request.Client,
                request.Subject);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            return new UserInfoResponse(profiles);
        }
    }
}
