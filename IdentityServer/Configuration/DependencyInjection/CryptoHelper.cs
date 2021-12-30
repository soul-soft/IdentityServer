using System.Security.Cryptography;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Configuration
{
    public static class CryptoHelper
    {
        public static RsaSecurityKey CreateRsaSecurityKey(int keySize = 2048)
        {
            return new RsaSecurityKey(RSA.Create(keySize))
            {
                KeyId = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)
            };
        }
      
        internal static string GetRsaSigningAlgorithmValue(IdentityServerConstants.RsaSigningAlgorithm value)
        {
            return value switch
            {
                IdentityServerConstants.RsaSigningAlgorithm.RS256 => SecurityAlgorithms.RsaSha256,
                IdentityServerConstants.RsaSigningAlgorithm.RS384 => SecurityAlgorithms.RsaSha384,
                IdentityServerConstants.RsaSigningAlgorithm.RS512 => SecurityAlgorithms.RsaSha512,

                IdentityServerConstants.RsaSigningAlgorithm.PS256 => SecurityAlgorithms.RsaSsaPssSha256,
                IdentityServerConstants.RsaSigningAlgorithm.PS384 => SecurityAlgorithms.RsaSsaPssSha384,
                IdentityServerConstants.RsaSigningAlgorithm.PS512 => SecurityAlgorithms.RsaSsaPssSha512,
                _ => throw new ArgumentException("Invalid RSA signing algorithm value", nameof(value)),
            };
        }
       
        internal static string GetCrvValueFromCurve(ECCurve curve)
        {
            return curve.Oid.Value switch
            {
                Constants.CurveOids.P256 => JsonWebKeyECTypes.P256,
                Constants.CurveOids.P384 => JsonWebKeyECTypes.P384,
                Constants.CurveOids.P521 => JsonWebKeyECTypes.P521,
                _ => throw new InvalidOperationException($"Unsupported curve type of {curve.Oid.Value} - {curve.Oid.FriendlyName}"),
            };
        }
    }
}
