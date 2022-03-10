using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {
        public Task<IEnumerable<Claim>> GetClaimDataAsync(ClaimDataRequestContext context)
        {
            IEnumerable<Claim> result = Array.Empty<Claim>();
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Profile>> GetUserInfoAsync(UserInfoRequestContext context)
        {
            IEnumerable<Profile> result = Array.Empty<Profile>();
            return Task.FromResult(result);
        }
    }
}
