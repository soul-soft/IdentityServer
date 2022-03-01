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

        public Task<Dictionary<string, object?>> GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var result = new Dictionary<string, object?>();
            return Task.FromResult(result);
        }
    }
}
