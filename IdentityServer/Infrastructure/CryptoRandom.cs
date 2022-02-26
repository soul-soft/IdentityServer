using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Infrastructure
{
    public static class CryptoRandom
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
     
        public static byte[] CreateRandomKey(int length)
        {
            byte[] array = new byte[length];
            Rng.GetBytes(array);
            return array;
        }

        public static string CreateUniqueId(int length = 32, OutputFormat format= OutputFormat.Base64Url)
        {
            byte[] array = CreateRandomKey(length);

            return format switch
            {
                OutputFormat.Base64Url => Base64UrlEncoder.Encode(array),
                OutputFormat.Base64 => Convert.ToBase64String(array),
                OutputFormat.Hex => BitConverter.ToString(array).Replace("-", ""),
                _ => throw new ArgumentException("Invalid output format", nameof(format)),
            };
        }

        public static RsaSecurityKey CreateRsaSecurityKey(int keySize = 2048)
        {
            return new RsaSecurityKey(RSA.Create(keySize))
            {
                KeyId = CreateUniqueId(16, OutputFormat.Hex)
            };
        }
      
        public enum OutputFormat
        {
            Base64Url,
            Base64,
            Hex
        }
    }
}
