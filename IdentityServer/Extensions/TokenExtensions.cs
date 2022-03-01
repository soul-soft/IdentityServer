//using System.Security.Claims;

//namespace IdentityServer.Models
//{
//    public static class TokenExtensions
//    {
//        public static IEnumerable<Claim> CreateJwtPayload(this Token token, IdentityServerOptions options)
//        {
//            var claims = new List<Claim>();
//            if (!string.IsNullOrWhiteSpace(token.Id))
//            {
//                claims.Add(new Claim(JwtClaimTypes.JwtId, token.Id));
//            }
//            if (!string.IsNullOrWhiteSpace(token.Issuer))
//            {
//                claims.Add(new Claim(JwtClaimTypes.Issuer, token.Issuer));
//            }
//            if (token.CreationTime.HasValue)
//            {
//                var time =  token.CreationTime.Value;
//                var now = new DateTimeOffset(time).ToUnixTimeSeconds().ToString();
//                var exp = now + token.Lifetime;
//                claims.Add(new Claim(JwtClaimTypes.NotBefore, now));
//                claims.Add(new Claim(JwtClaimTypes.IssuedAt, now));
//                claims.Add(new Claim(JwtClaimTypes.Expiration, exp));
//            }
//            if (!string.IsNullOrWhiteSpace(token.ClientId))
//            {
//                claims.Add(new Claim(JwtClaimTypes.ClientId, token.ClientId));
//            }
//            foreach (var item in token.Audiences)
//            {
//                claims.Add(new Claim(JwtClaimTypes.Audience, item));
//            }
//            if (options.EmitScopesAsSpaceDelimitedStringInJwt)
//            {
//                claims.Add(new Claim(JwtClaimTypes.Scope, string.Join(",", token.Scopes)));
//            }
//            else
//            {
//                foreach (var scope in token.Scopes)
//                {
//                    claims.Add(new Claim(JwtClaimTypes.Scope, scope));
//                }
//            }
//            foreach (var item in token.Claims)
//            {
//                if (!claims.Exists(a => a.Type == item.Type))
//                {
//                    claims.Add(item);
//                }
//            }
//            return claims;
//        }
//    }
//}
