using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Application
{
    internal class ClientSecretValidator
        : IClientSecretValidator
    {
        private readonly ISystemClock _systemClock;

        private readonly ICredentialValidator _credentialValidator;

        public ClientSecretValidator(
            ISystemClock systemClock,
            ICredentialValidator credentialValidator)
        {
            _systemClock = systemClock;
            _credentialValidator = credentialValidator;
        }

        public async Task<ValidationResult> ValidateAsync(IClient client, ParsedCredential credential)
        {
            if (!client.Enabled)
            {
                return ValidationResult.Error("Client not enabled");
            }
            var secrets = client.ClientSecrets
               .Where(a => a.Enabled)
               .Where(a => a.Expiration.HasValue && !a.Expiration.Value.HasExpired(_systemClock.UtcNow.UtcDateTime));
            //验证密钥
            var validationResult = await _credentialValidator.ValidateAsync(secrets, credential);
            if (validationResult.IsError)
            {
                return validationResult;
            }
            return ValidationResult.Success();
        }
    }
}
