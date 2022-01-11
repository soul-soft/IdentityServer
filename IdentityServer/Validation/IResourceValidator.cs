using IdentityServer.Models;

namespace IdentityServer.Validation
{
    public interface IResourceValidator
    {
        Task<ValidationResult> ValidateAsync(Resources resources, IEnumerable<string> requestedScopes);
    }
}
