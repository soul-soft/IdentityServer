using IdentityServer.Models;

namespace IdentityServer.Application
{
    public interface IClientSecretValidator
    {
        Task<ValidationResult> ValidateAsync(IClient client, ParsedCredential credential);
    }
}
