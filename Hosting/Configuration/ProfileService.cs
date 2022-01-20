using System.Security.Claims;
using IdentityServer.Models;
using IdentityServer.Services;

namespace Hosting.Configuration
{
    public class ProfileService: IProfileService
    {
        public Task<IEnumerable<Claim>> GetUserClaimsAsync(UserClaimsProfileRequest request)
        {
            return Task.FromResult<IEnumerable<Claim>>(new List<Claim>());
        }

        public Task<Dictionary<string, object?>> GetUserInfoAsync(UserInfoProfileRequest request)
        {
            var datas = new Dictionary<string, object?>();
            datas.Add("name","zs");
            return Task.FromResult(datas);
        }
    }
}
