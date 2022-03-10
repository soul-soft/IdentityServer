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
            var profileDataRequestContext = new UserInfoRequestContext(
                request.Client,
                request.Subject);
            var profiles = await _profileService.GetUserInfoAsync(profileDataRequestContext);
            return new UserInfoResponse(profiles);
        }
    }
}
