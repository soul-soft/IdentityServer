using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServer.Services
{
    internal class CodeChallengeService : ICodeChallengeService
    {
        public string ComputeHash(string code, string method)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(code);
                var hash = sha.ComputeHash(bytes);
                return Base64UrlEncoder.Encode(hash);
            }
        }
    }
}
