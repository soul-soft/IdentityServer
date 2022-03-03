using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Models
{
    public static class TokenExtensions
    {
        public static IEnumerable<Claim> GetJwtPayloadClaims(this Token token)
        {
            var now = new DateTimeOffset(token.CreationTime).ToUnixTimeSeconds();
            var exp = now + token.Lifetime;
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.JwtId, token.Id));
            claims.Add(new Claim(JwtClaimTypes.NotBefore, now.ToString()));
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, now.ToString()));
            claims.Add(new Claim(JwtClaimTypes.Expiration, exp.ToString()));
            claims.AddRange(token.Claims);
            return claims;
        }
    }
}
