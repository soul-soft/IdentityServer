using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {
        public Task<bool> IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequestContext context)
        {
            IEnumerable<Claim> result = Array.Empty<Claim>();
            return Task.FromResult(result);
        }
    }
}
