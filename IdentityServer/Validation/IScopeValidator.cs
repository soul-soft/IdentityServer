using IdentityServer.Models;

namespace IdentityServer.Validation
{
    public interface IScopeValidator
    {
        Task<ValidationResult> Validate(IEnumerable<string> allowedScopes,IEnumerable<string> requestedScopes);
    }
}
