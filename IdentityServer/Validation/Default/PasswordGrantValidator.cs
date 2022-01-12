using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Validation
{
    internal class PasswordGrantValidator : IPasswordGrantValidator
    {
        public Task<ValidationResult> ValidateAsync(PasswordGrantContext context)
        {
            //"invalid username or password";
            return ValidationResult.SuccessAsync();
        }
    }
}
