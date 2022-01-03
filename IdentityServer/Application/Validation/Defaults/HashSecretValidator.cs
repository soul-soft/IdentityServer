using IdentityServer.Models;

namespace IdentityServer.Application
{
    internal class HashSecretValidator : ISecretValidator
    {
        public Task<ValidationResult> ValidateAsync(SecretValidationRequest request)
        {
            var credential = request.ParsedSecret.Credential?.ToString();
            if (request.ParsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
            {
                return ValidationResult.ErrorAsync("Hashed shared secret validator cannot process {type}", request.ParsedSecret.Type ?? "null");
            }
            if (string.IsNullOrEmpty(request.ParsedSecret.Id) || string.IsNullOrEmpty(credential))
            {
                throw new ArgumentException("Id or Credential is missing.");
            }
            var sharedSecrets = request.AllowedSecrets
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
                    return ValidationResult.SuccessAsync();
                }
            }
            return ValidationResult.ErrorAsync("No matching hashed secret found.");
        }
    }
}
