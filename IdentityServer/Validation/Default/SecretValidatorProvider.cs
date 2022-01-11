using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Validation
{
    public class SecretValidatorProvider
        : ISecretValidatorProvider
    {
        private readonly IEnumerable<ISecretValidator> _secretValidators;
       
        public SecretValidatorProvider(IEnumerable<ISecretValidator> secretValidators)
        {
            _secretValidators = secretValidators;
        }

        public ISecretValidator GetSecretValidator(string secretType)
        {
            return _secretValidators
                .Where(a => a.SecretType == secretType)
                .First();
        }
    }
}
