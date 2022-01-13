using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Models
{
    public static class IClaimLiteExtensions
    {
        public static IEnumerable<Claim> ToClaims(this ICollection<IClaimLite> claims)
        {
            return claims.Select(s => new Claim(s.Name, s.Value, s.ValueType));
        }
    }
}
