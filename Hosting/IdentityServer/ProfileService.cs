using IdentityServer.Models;
using IdentityServer.Services;

namespace Hosting.Configuration
{
    public class ProfileService : IProfileService
    {
        Task<IEnumerable<Profile>> IProfileService.GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var profiles = new List<Profile>();
            if (context.Caller == ProfileDataCallers.UserInfoEndpoint)
            {
                profiles.Add(new Profile("name", "zs"));
                profiles.Add(new Profile("age", 50));
                profiles.Add(new Profile("birthday", DateTime.Now));
                profiles.Add(new Profile("isok", true));
            }
            else
            {
                if (context.ClaimTypes.Contains(JwtClaimTypes.Subject))
                {
                    profiles.Add(new Profile(JwtClaimTypes.Subject, 10));
                }
                if (context.ClaimTypes.Contains(JwtClaimTypes.Role))
                {
                    profiles.Add(new Profile(JwtClaimTypes.Role, "admin"));
                }
            }
            return Task.FromResult<IEnumerable<Profile>>(profiles);
        }
    }
}
