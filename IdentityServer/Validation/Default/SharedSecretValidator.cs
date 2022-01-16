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
                throw new UnauthorizedClientException("Client credential is missing");
            }

            var credentials = new string[]
            {
                credential.Sha512(),
                credential.Sha256(),
            };

            var secets = allowedSecrets
                .Where(a => credentials.Contains(a.Value));

            if (secets.Any(a => a.Expiration == null || a.Expiration >= _clock.UtcNow.UtcDateTime))
            {
                return Task.CompletedTask;
            }
            else if (secets.Any(a => a.Expiration != null || a.Expiration < _clock.UtcNow.UtcDateTime))
            {
                throw new UnauthorizedClientException("The Client credential has expired");
            }
            else
            {
                throw new UnauthorizedClientException("Invalid client or credential");
            }
        }
    }
}
