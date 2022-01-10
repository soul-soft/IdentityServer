using IdentityServer.Infrastructure;
using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Endpoints
{
    public class JwkDiscoveryResponse
    {
        public IEnumerable<JsonWebKey> Jwks { get; }

        public JwkDiscoveryResponse(IEnumerable<JsonWebKey> jwks)
        {
            Jwks = jwks;
        }

        public string Serialize()
        {
            var keys = Jwks.Select(jwk =>
            {
                return Map(jwk);
            }).ToArray();
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
                KeyOps = jsonWebKey.ShouldSerializeKeyOps() ? jsonWebKey.KeyOps : null,
                jsonWebKey.Kid,
                jsonWebKey.Kty,
                jsonWebKey.N,
                Oth = jsonWebKey.Oth,
                jsonWebKey.P,
                jsonWebKey.Q,
                jsonWebKey.QI,
                Use = jsonWebKey.Use ?? "sig",
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
