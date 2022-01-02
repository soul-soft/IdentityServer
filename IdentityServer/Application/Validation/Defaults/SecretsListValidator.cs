using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Application
{
    internal class SecretsListValidator 
        : ISecretsListValidator
    {
        private readonly ISystemClock _clock;
        
        private readonly IEnumerable<ISecretValidator> _validators;
     
        public SecretsListValidator(ISystemClock clock,IEnumerable<ISecretValidator> validators)
        {
            _clock = clock;
            _validators = validators;
        }

        public async Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
        {
            SecretValidationResult result;
            var availableSecrets = secrets.Where(a => a.Enabled)
                .Where(a => a.Expiration.HasValue && !a.Expiration.Value.HasExpired(_clock.UtcNow.UtcDateTime));
            foreach (var validator in _validators)
            {
                result = await validator.ValidateAsync(availableSecrets, parsedSecret);
                if (!result.IsError)
                {
                    return result;
                }
            }
            result = new SecretValidationResult();
            result.Error("Secret validators could not validate secret");
            return result;
        }
    }
}
