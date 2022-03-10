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
            if (claims.Exists(a => a.Type == JwtClaimTypes.Subject))
            {
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, now.ToString()));
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, token.IdentityProvider));
                claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, token.GrantType));
            }
            claims.AddRange(token.Claims);
            return claims;
        }
    }
}
