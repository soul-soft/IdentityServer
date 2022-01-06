using IdentityServer.Models;

namespace IdentityServer.Application
{
    public interface ICredentialValidator
    {
        Task<ValidationResult> ValidateAsync(IEnumerable<ISecret> secrets, ParsedCredential credential);
    }
}
