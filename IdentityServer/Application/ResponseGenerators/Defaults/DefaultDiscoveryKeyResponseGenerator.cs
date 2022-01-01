using System.Security.Cryptography;
using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = IdentityServer.Models.JsonWebKey;

namespace IdentityServer.Application
{
    internal class DefaultDiscoveryKeyResponseGenerator
        : IDiscoveryKeyResponseGenerator
    {
        private readonly ISigningCredentialStore _signingCredentials;

        public DefaultDiscoveryKeyResponseGenerator(
            ISigningCredentialStore signingCredentials)
        {
            _signingCredentials = signingCredentials;
        }


        public async Task<IEnumerable<JsonWebKey>> CreateJwkDocumentAsync()
        {
            var webKeys = new List<JsonWebKey>();

            foreach (var key in await _signingCredentials.GetSecurityKeyInfosAsync())
            {
                if (key.Key is X509SecurityKey x509Key)
                {
                    var cert64 = Convert.ToBase64String(x509Key.Certificate.RawData);
                    var thumbprint = Base64Url.Encode(x509Key.Certificate.GetCertHash());

                    if (x509Key.PublicKey is RSA rsa)
                    {
                        var parameters = rsa.ExportParameters(false);
                        var exponent = Base64Url.Encode(parameters.Exponent);
                        var modulus = Base64Url.Encode(parameters.Modulus);

                        var rsaJsonWebKey = new JsonWebKey
                        {
                            kty = "RSA",
                            use = "sig",
                            kid = x509Key.KeyId,
                            x5t = thumbprint,
                            e = exponent,
                            n = modulus,
                            x5c = new[] { cert64 },
                            alg = key.SigningAlgorithm
                        };
                        webKeys.Add(rsaJsonWebKey);
                    }
                    else if (x509Key.PublicKey is ECDsa ecdsa)
                    {
                        var parameters = ecdsa.ExportParameters(false);
                        var x = Base64Url.Encode(parameters.Q.X);
                        var y = Base64Url.Encode(parameters.Q.Y);

                        var ecdsaJsonWebKey = new JsonWebKey
                        {
                            kty = "EC",
                            use = "sig",
                            kid = x509Key.KeyId,
                            x5t = thumbprint,
                            x = x,
                            y = y,
                            crv = CryptoHelper.GetCrvValueFromCurve(parameters.Curve),
                            x5c = new[] { cert64 },
                            alg = key.SigningAlgorithm
                        };
                        webKeys.Add(ecdsaJsonWebKey);
                    }
                    else
                    {
                        throw new InvalidOperationException($"key type: {x509Key.PublicKey.GetType().Name} not supported.");
                    }
                }
                else if (key.Key is RsaSecurityKey rsaKey)
                {
                    var parameters = rsaKey.Rsa?.ExportParameters(false) ?? rsaKey.Parameters;
                    var exponent = Base64Url.Encode(parameters.Exponent);
                    var modulus = Base64Url.Encode(parameters.Modulus);

                    var webKey = new JsonWebKey
                    {
                        kty = "RSA",
                        use = "sig",
                        kid = rsaKey.KeyId,
                        e = exponent,
                        n = modulus,
                        alg = key.SigningAlgorithm
                    };

                    webKeys.Add(webKey);
                }
                else if (key.Key is ECDsaSecurityKey ecdsaKey)
                {
                    var parameters = ecdsaKey.ECDsa.ExportParameters(false);
                    var x = Base64Url.Encode(parameters.Q.X);
                    var y = Base64Url.Encode(parameters.Q.Y);

                    var ecdsaJsonWebKey = new JsonWebKey
                    {
                        kty = "EC",
                        use = "sig",
                        kid = ecdsaKey.KeyId,
                        x = x,
                        y = y,
                        crv = CryptoHelper.GetCrvValueFromCurve(parameters.Curve),
                        alg = key.SigningAlgorithm
                    };
                    webKeys.Add(ecdsaJsonWebKey);
                }
                else if (key.Key is Microsoft.IdentityModel.Tokens.JsonWebKey jsonWebKey)
                {
                    var webKey = new JsonWebKey
                    {
                        kty = jsonWebKey.Kty,
                        use = jsonWebKey.Use ?? "sig",
                        kid = jsonWebKey.Kid,
                        x5t = jsonWebKey.X5t,
                        e = jsonWebKey.E,
                        n = jsonWebKey.N,
                        x5c = jsonWebKey.X5c?.Count == 0 ? null : jsonWebKey.X5c?.ToArray(),
                        alg = jsonWebKey.Alg,
                        crv = jsonWebKey.Crv,
                        x = jsonWebKey.X,
                        y = jsonWebKey.Y
                    };

                    webKeys.Add(webKey);
                }
            }

            return webKeys;
        }
    }
}
