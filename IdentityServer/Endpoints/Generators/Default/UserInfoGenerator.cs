namespace IdentityServer.Endpoints
{
    internal class UserInfoGenerator : IUserInfoGenerator
    {
        private readonly IProfileService _profileService;

        public UserInfoGenerator(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<UserInfoResponse> ProcessAsync(UserInfoRequest request)
        {
            var userinfo = await _profileService.GetUserInfoAsync(new UserInfoProfileRequest(
                request.Subject,
                request.Client,
                request.Resources));
            var response = new UserInfoResponse(userinfo);
            return response;
        }
    }
}
