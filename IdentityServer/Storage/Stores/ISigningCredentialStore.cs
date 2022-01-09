using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        Task<IEnumerable<SigningCredentialsDescriptor>> GetSigningCredentialsAsync();
        Task<SigningCredentialsDescriptor> GetSigningCredentialsByAlgorithmsAsync(string? algorithm);
    }
}
