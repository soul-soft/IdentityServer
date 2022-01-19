using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {

        public Task<IEnumerable<Claim>> GetUserClaimsAsync(UserClaimsProfileRequest request)
        {
            return Task.FromResult<IEnumerable<Claim>>(new List<Claim>());
        }

        public Task<Dictionary<string, object?>> GetUserInfoAsync(UserInfoProfileRequest request)
        {
            return Task.FromResult(new Dictionary<string, object?>());
        }
    }
}
