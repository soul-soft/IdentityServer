using IdentityServer.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Endpoints
{
    public class JwkDiscoveryResponse
    {
        public IEnumerable<SecurityKey> SecurityKeys { get; }

        public JwkDiscoveryResponse(IEnumerable<SecurityKey> securityKeys)
        {
            SecurityKeys = securityKeys;
        }

        public string Serialize()
        {
            var keys = SecurityKeys.Select(key =>
            {
                var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(key);
                return Map(jwk);
            });
            var keySet = new { Keys = keys };
            return ObjectSerializer.SerializeObject(keySet);
        }

        public static object Map(JsonWebKey jsonWebKey)
        {
            return new
            {
                jsonWebKey.Alg,
                jsonWebKey.Crv,
                jsonWebKey.D,
                jsonWebKey.DP,
                jsonWebKey.DQ,
                jsonWebKey.E,
                jsonWebKey.K,
                jsonWebKey.KeyId,
                KeyOps = jsonWebKey.ShouldSerializeKeyOps() ? jsonWebKey.KeyOps : null,
                jsonWebKey.Kid,
                jsonWebKey.Kty,
                jsonWebKey.N,
                Oth = jsonWebKey.Oth,
                jsonWebKey.P,
                jsonWebKey.Q,
                jsonWebKey.QI,
                jsonWebKey.Use,
                jsonWebKey.X,
                X5c = jsonWebKey.ShouldSerializeX5c() ? jsonWebKey.X5c : null,
                jsonWebKey.X5t,
                jsonWebKey.X5tS256,
                jsonWebKey.X5u,
                jsonWebKey.Y,
            };
        }
    }
}
