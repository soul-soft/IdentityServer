using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {
        public Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest context)
        {
            var list = new List<Claim>();
            return Task.FromResult<IEnumerable<Claim>>(list);
        }
    }
}
