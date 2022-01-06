using IdentityServer.Models;

namespace IdentityServer.Application
{
    public interface IScopeRequestValidator
    {
        Task<ValidationResult> ValidateAsync(IClient client, string[] scopes);
    }
}
