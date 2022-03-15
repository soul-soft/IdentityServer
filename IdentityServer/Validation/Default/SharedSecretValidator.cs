using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Validation
{
    internal class SharedSecretValidator : ISecretValidator
    {
        public string CredentialsType => ClientSecretTypes.SharedSecret;

        private readonly ISystemClock _clock;

        public SharedSecretValidator(ISystemClock clock)
        {
            _clock = clock;
        }

        public Task ValidateAsync(ParsedSecret clientCredentials, IEnumerable<Secret> allowedSecrets)
        {
            var credential = clientCredentials.Credentials.ToString();

            if (string.IsNullOrEmpty(credential))
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Invalid client credentials");
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
                throw new ValidationException(OpenIdConnectErrors.UnauthorizedClient, "Invalid client credentials");
            }

            if (availableSecets.Any(a => a.Expiration == null || a.Expiration >= _clock.UtcNow.UtcDateTime))
            {
                return Task.CompletedTask;
            }
            else
            {
                throw new ValidationException(OpenIdConnectErrors.UnauthorizedClient, "The client credentials has expired");
            }
        }
    }
}
