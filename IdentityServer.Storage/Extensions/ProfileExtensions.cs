using System.Security.Claims;

namespace IdentityServer.Models
{
    public static class ProfileExtensions
    {
        public static IEnumerable<Claim> ToClaims(this IEnumerable<Profile> profiles)
        {
            foreach (var item in profiles)
            {
                yield return (Claim)item;
            }
        }

        public static IEnumerable<Profile> ToProfiles(this IEnumerable<Claim> claims)
        {
            foreach (var item in claims)
            {
                yield return (Profile) item;
            }
        }
    }
}
