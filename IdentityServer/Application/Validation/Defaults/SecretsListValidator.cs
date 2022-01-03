using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Application
{
    internal class SecretsListValidator
        : ISecretListValidator
    {
        private readonly ISystemClock _clock;

        private readonly IEnumerable<ISecretValidator> _validators;

        public SecretsListValidator(ISystemClock clock, IEnumerable<ISecretValidator> validators)
        {
            _clock = clock;
            _validators = validators;
        }

        public async Task<ValidationResult> ValidateAsync(SecretValidationRequest request)
        {
            var allowedSecrets = request.AllowedSecrets
                .Where(a => a.Enabled)
                .Where(a => a.Expiration.HasValue && !a.Expiration.Value.HasExpired(_clock.UtcNow.UtcDateTime));
          
            foreach (var validator in _validators)
            {
                var validateResult = await validator
                    .ValidateAsync(new SecretValidationRequest(allowedSecrets, request.ParsedSecret));

                if (!validateResult.IsError)
                {
                    return validateResult;
                }
            }
            return ValidationResult.Error("Secret validators could not validate secret");
        }
    }
}
