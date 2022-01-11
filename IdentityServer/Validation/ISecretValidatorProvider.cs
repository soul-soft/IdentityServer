using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Validation
{
    public interface ISecretValidatorProvider
    {
        ISecretValidator GetSecretValidator(string secretType);
    }
}
