using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Validation
{
    internal class SharedClientCredentialsValidator : IClientCredentialsValidator
    {
        public string CredentialsType => ClientSecretTypes.SharedSecret;

        private readonly ISystemClock _clock;

        public SharedClientCredentialsValidator(ISystemClock clock)
        {
            _clock = clock;
        }

        public Task ValidateAsync(ClientCredentials clientCredentials, IEnumerable<Secret> allowedSecrets)
        {
            var credential = clientCredentials.Credentials.ToString();

            if (string.IsNullOrEmpty(credential))
            {
                throw new InvalidRequestException("Invalid clientCredentials");
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
                throw new InvalidRequestException("Invalid clientCredentials");
            }

            if (availableSecets.Any(a => a.Expiration == null || a.Expiration >= _clock.UtcNow.UtcDateTime))
            {
                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidRequestException("The clientCredentials has expired");
            }
        }
    }
}
