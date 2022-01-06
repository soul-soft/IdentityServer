using IdentityServer.Models;

namespace IdentityServer.Application
{
    internal class SharedSecretValidator 
        : ICredentialValidator
    {
        public Task<ValidationResult> ValidateAsync(IEnumerable<ISecret> secrets, ParsedCredential credential)
        {
            if (credential.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
            {
                return ValidationResult.ErrorAsync("Hashed shared secret validator cannot process {type}", credential.Type ?? "null");
            }
            var secretHash = credential.Secret?.ToString();
            if (string.IsNullOrEmpty(credential.Id) || string.IsNullOrEmpty(secretHash))
            {
                throw new ArgumentException("Id or Credential is missing.");
            }
            var sharedSecrets = secrets
                .Where(s => s.Type == IdentityServerConstants.SecretTypes.SharedSecret);
            var sha256Credential = secretHash.Sha256();
            var sha512Credential = secretHash.Sha512();
            var credentials = new string[]
            {
                sha256Credential,
                sha512Credential
            };
            foreach (var secret in sharedSecrets)
            {
                if (credentials.Contains(secret.Value))
                {
                    return ValidationResult.SuccessAsync();
                }
            }
            return ValidationResult.ErrorAsync("No matching hashed secret found.");
        }
    }
}
