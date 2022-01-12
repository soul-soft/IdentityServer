using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using static IdentityServer.OpenIdConnects;

namespace IdentityServer.Validation
{
    internal class SharedSecretValidator : ISecretValidator
    {
        public string SecretType => ClientSecretTypes.SharedSecret;

        private readonly ISystemClock _clock;

        public SharedSecretValidator(ISystemClock clock)
        {
            _clock = clock;
        }

        public Task<ValidationResult> ValidateAsync(IEnumerable<ISecret> secrets, ClientSecret clientSecret)
        {
            var credential = clientSecret.Credential?.ToString();

            if (string.IsNullOrWhiteSpace(credential))
            {
                return ValidationResult.ErrorAsync("Client credential is missing");
            }

            var credentials = new string[]
            {
                credential.Sha512(),
                credential.Sha256(),
            };

            var equalsSecets = secrets
                .Where(a => credentials.Contains(a.Value));

            if (equalsSecets.Any(a => a.Expiration == null || a.Expiration >= _clock.UtcNow.UtcDateTime))
            {
                return ValidationResult.SuccessAsync();
            }
            else if (equalsSecets.Any(a => a.Expiration != null || a.Expiration < _clock.UtcNow.UtcDateTime))
            {
                return ValidationResult.ErrorAsync("The Client credential has expired");
            }
            else
            {
                return ValidationResult.ErrorAsync("Invalid client or credential");
            }
        }
    }
}
