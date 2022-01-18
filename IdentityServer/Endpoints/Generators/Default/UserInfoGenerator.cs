namespace IdentityServer.Endpoints
{
    internal class UserInfoGenerator : IUserInfoGenerator
    {
        public Task<UserInfoResponse> ProcessAsync(UserInfoRequest request)
        {
            var response = new UserInfoResponse();
            return Task.FromResult(response);
        }
    }
}
