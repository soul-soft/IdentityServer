using System.Security.Claims;

namespace IdentityServer.Models
{
    public static class ClaimLiteExtensions
    {
        public static IEnumerable<Claim> ToClaims(this IEnumerable<ClaimLite> claims)
        {
            return claims.Select(s => new Claim(s.Type, s.Value, s.ValueType));
        }
        public static IEnumerable<ClaimLite> ToClaimLites(this IEnumerable<Claim> claims)
        {
            return claims.Select(s => new ClaimLite(s.Type, s.Value, s.ValueType));
        }
    }
}
