using Microsoft.AspNetCore.Authentication;

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

        public Task ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets)
        {
            var credential = clientSecret.Credential?.ToString();

            if (string.IsNullOrWhiteSpace(credential))
            {
                throw new InvalidRequestException("Invalid client or credential");
            }

            var credentials = new string[]
            {
                credential.Sha512(),
                credential.Sha256(),
            };

            var availableSecets = allowedSecrets
                .Where(a => credentials.Contains(a.Value));

            if (!availableSecets.Any())
            {
                throw new InvalidRequestException("Invalid client or credential");
            }

            if (availableSecets.Any(a => a.Expiration == null || a.Expiration >= _clock.UtcNow.UtcDateTime))
            {
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidRequestException("The Client credential has expired");
            }
        }
    }
}
