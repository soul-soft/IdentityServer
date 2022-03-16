using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ProfileService : IProfileService
    {
        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequestContext context)
        {
            IEnumerable<Claim> result = Array.Empty<Claim>();
            return Task.FromResult(result);
        }
    }
}
