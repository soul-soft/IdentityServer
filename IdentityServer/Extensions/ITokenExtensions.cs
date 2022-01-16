using System.Security.Claims;

namespace IdentityServer.Models
{
    public static class ITokenExtensions
    {
        public static IEnumerable<Claim> ToClaims(this IAccessToken token, IdentityServerOptions options)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrWhiteSpace(token.Id))
            {
                claims.Add(new Claim(JwtClaimTypes.JwtId, token.Id));
            }
            if (!string.IsNullOrWhiteSpace(token.Issuer))
            {
                claims.Add(new Claim(JwtClaimTypes.Issuer, token.Issuer));
            }
            if (token.NotBefore.HasValue)
            {
                var timestamp = new DateTimeOffset(token.NotBefore.Value).ToUnixTimeSeconds();
                claims.Add(new Claim(JwtClaimTypes.NotBefore, timestamp.ToString()));
            }
            if (token.Expiration.HasValue)
            {
                var timestamp = new DateTimeOffset(token.Expiration.Value).ToUnixTimeSeconds();
                claims.Add(new Claim(JwtClaimTypes.Expiration, timestamp.ToString()));
            }
            if (!string.IsNullOrWhiteSpace(token.ClientId))
            {
                claims.Add(new Claim(JwtClaimTypes.ClientId, token.ClientId));
            }
            if (!string.IsNullOrWhiteSpace(token.GrantType))
            {
                claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, token.GrantType));
            }
            if (!string.IsNullOrWhiteSpace(token.SubjectId))
            {
                claims.Add(new Claim(JwtClaimTypes.Subject, token.SubjectId));
            }
            if (!string.IsNullOrEmpty(token.SessionId))
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, token.SessionId));
            }
            if (token.Audiences.Any())
            {
                foreach (var item in token.Audiences)
                {
                    claims.Add(new Claim(JwtClaimTypes.Audience, item));
                }
            }
            if (options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                claims.Add(new Claim(JwtClaimTypes.Scope, string.Join(",", token.Scopes)));
            }
            else
            {
                foreach (var scope in token.Scopes)
                {
                    claims.Add(new Claim(JwtClaimTypes.Scope, scope));
                }
            }
            foreach (var item in token.Claims)
            {
                if (!claims.Exists(a => a.Type == item.Type))
                {
                    claims.Add(new Claim(item.Type, item.Value, item.ValueType));
                }
            }
            return claims;
        }
    }
}
