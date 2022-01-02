using IdentityServer.Infrastructure;
using IdentityServer.Models;

namespace IdentityServer.Application
{
    /// <summary>
    /// hash密钥凭据验证
    /// </summary>
    internal class HashedSharedSecretValidator : ISecretValidator
    {
        public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
        {
            var result = new SecretValidationResult();
            var credential = parsedSecret.Credential?.ToString();
            if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
            {
                return Error("Hashed shared secret validator cannot process {type}", parsedSecret.Type ?? "null");
            }
            if (string.IsNullOrEmpty(parsedSecret.Id) || string.IsNullOrEmpty(credential))
            {
                throw new ArgumentException("Id or Credential is missing.");
            }
            var sharedSecrets = secrets
                .Where(s => s.Type == IdentityServerConstants.SecretTypes.SharedSecret);
            var sha256Credential = credential.Sha256();
            var sha512Credential = credential.Sha512();
            var credentials = new string[] 
            { 
                sha256Credential, 
                sha512Credential 
            };
            foreach (var secret in sharedSecrets)
            {
                if (credentials.Contains(secret.Value))
                {
                    return Success();
                }
            }
            return Error("No matching hashed secret found.");
        }

        private Task<SecretValidationResult> Error(string format, params object[] args)
        {
            var result = new SecretValidationResult();
            result.Error(string.Format(format, args));
            return Task.FromResult(result);
        }
        private Task<SecretValidationResult> Success()
        {
            var result = new SecretValidationResult();
            return Task.FromResult(result);
        }
    }
}
