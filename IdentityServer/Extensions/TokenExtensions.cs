using System.Security.Claims;

namespace IdentityServer.Models
{
    internal static class TokenExtensions
    {
        public static IEnumerable<Claim> GetJwtClaims(this Token token)
        {
            var creationTime = new DateTimeOffset(token.CreationTime).ToUnixTimeSeconds();
            var exp = creationTime + token.Lifetime;
            var claims = new List<Claim>(token.Claims);
            claims.Add(new Claim(JwtClaimTypes.JwtId, token.Id));
            claims.Add(new Claim(JwtClaimTypes.NotBefore, creationTime.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, creationTime.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.Expiration, exp.ToString(), ClaimValueTypes.Integer64));
            if (!string.IsNullOrEmpty(token.SubjectId))
            {
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, creationTime.ToString(), ClaimValueTypes.Integer64));
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, token.IdentityProvider));
                claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, token.GrantType));
            }
            return claims;
        }
    }
}
