using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {
        public Task<IEnumerable<Claim>> GetProfileDataAsync(Resources resources)
        {
            return Task.FromResult<IEnumerable<Claim>>(new List<Claim>());
        }
    }
}
