using System.Security.Claims;

namespace IdentityServer.Models
{
    internal static class TokenExtensions
    {
        //public static IEnumerable<Claim> GetJwtClaims(this Token token)
        //{
        //    var creationTime = new DateTimeOffset(token.IssuedAt).ToUnixTimeSeconds();
        //    var exp = creationTime + token.Lifetime;
        //    var claims = new List<Claim>(token.Claims);
        //    claims.Add(new Claim(JwtClaimTypes.JwtId, token.JwtId));
        //    claims.Add(new Claim(JwtClaimTypes.NotBefore, creationTime.ToString(), ClaimValueTypes.Integer64));
        //    claims.Add(new Claim(JwtClaimTypes.IssuedAt, creationTime.ToString(), ClaimValueTypes.Integer64));
        //    claims.Add(new Claim(JwtClaimTypes.Expiration, exp.ToString(), ClaimValueTypes.Integer64));
        //    return claims;
        //}
    }
}
