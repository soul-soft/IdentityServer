using IdentityServer.Models;

namespace IdentityServer.Application
{
    public interface ISecretValidator
    {
        Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret);
    }
}
