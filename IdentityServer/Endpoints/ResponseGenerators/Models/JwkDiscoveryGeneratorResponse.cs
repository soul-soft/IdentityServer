using IdentityServer.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Endpoints
{
    public class JwkDiscoveryGeneratorResponse
    {
        public IEnumerable<JsonWebKey> Jwks { get; }

        public JwkDiscoveryGeneratorResponse(IEnumerable<JsonWebKey> jwks)
        {
            Jwks = jwks;
        }

        public string Serialize()
        {
            var keys = Jwks.Select(jwk => MapKey(jwk))
                .ToArray();
            var keySet = new 
            { 
                Keys = keys 
            };
            return ObjectSerializer.Serialize(keySet);
        }

        public static object MapKey(JsonWebKey jsonWebKey)
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
                jsonWebKey.Oth,
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
