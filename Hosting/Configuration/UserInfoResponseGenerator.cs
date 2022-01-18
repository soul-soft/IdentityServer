using IdentityServer.Endpoints;

namespace Hosting.Configuration
{
    public class UserInfoResponseGenerator
        : IUserInfoGenerator
    {
        public Task<UserInfoResponse> ProcessAsync(UserInfoRequest request)
        {
            var response = new UserInfoResponse();
            response.Add("name", "zs");
            response.Add("age", 50);
            return Task.FromResult(response);
        }
    }
}
