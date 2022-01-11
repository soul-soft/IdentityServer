using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Validation
{
    public interface ISecretValidator
    {
        string SecretType { get; }
        Task<ValidationResult> ValidateAsync(IEnumerable<ISecret> secrets, ClientSecret clientSecret);
    }
}
