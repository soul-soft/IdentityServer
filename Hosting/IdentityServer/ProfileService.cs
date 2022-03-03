using IdentityServer.Models;
using IdentityServer.Services;
using System.Security.Claims;

namespace Hosting.Configuration
{
    public class ProfileService : IProfileService
    {
        public Task<IEnumerable<Claim>> GetClaimDataAsync(ClaimDataRequestContext context)
        {
            var profiles = new List<Claim>();
            if (context.ClaimTypes.Contains(JwtClaimTypes.Subject))
            {
                profiles.Add(new Claim(JwtClaimTypes.Subject, "10"));
            }
            if (context.ClaimTypes.Contains(JwtClaimTypes.Role))
            {
                profiles.Add(new Claim(JwtClaimTypes.Role, "admin"));
            }
            return Task.FromResult<IEnumerable<Claim>>(profiles);
        }

        public Task<Dictionary<string,object?>> GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var data = new Dictionary<string, object?>();
            data.Add(JwtClaimTypes.Subject, "10");
            data.Add(JwtClaimTypes.Role, "admin");
            return Task.FromResult(data);
        }
    }
}
