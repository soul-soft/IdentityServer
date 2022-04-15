using IdentityServer.Models;
using IdentityServer.Services;
using System.Security.Claims;

namespace Hosting.Configuration
{
    public class ProfileService : IProfileService
    {
        public Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest context)
        {
            var profiles = new List<Claim>();
            if (context.ClaimTypes.Contains(JwtClaimTypes.Subject))
            {
                profiles.Add(new Claim(JwtClaimTypes.Subject, "10"));
            }
            if (context.ClaimTypes.Contains(JwtClaimTypes.Role))
            {
                profiles.Add(new Claim(JwtClaimTypes.Role, "admin1"));
            }
            if (context.ClaimTypes.Contains(JwtClaimTypes.Role))
            {
                profiles.Add(new Claim(JwtClaimTypes.Role, "admin2"));
            }
            if (context.ClaimTypes.Contains(JwtClaimTypes.Email))
            {
                var email = new { account = "123", cpy = "126" };
                var json = System.Text.Json.JsonSerializer.Serialize(email);
                profiles.Add(new Claim(JwtClaimTypes.Email, json, "json"));
            }
            return Task.FromResult<IEnumerable<Claim>>(profiles);
        }

        public Task<bool> IsActiveAsync(IsActiveRequest context)
        {
            return Task.FromResult(true);
        }
    }
}
