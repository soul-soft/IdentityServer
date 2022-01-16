namespace IdentityServer.Endpoints
{
    internal class UserInfoResponseGenerator : IUserInfoResponseGenerator
    {
        public Task<UserInfoResponse> ProcessAsync(UserInfoRequest request)
        {
            var response = new UserInfoResponse();
            return Task.FromResult(response);
        }
    }
}
