using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync();
        Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(IEnumerable<string> algorithms);
        Task<IEnumerable<SigningCredentialsDescriptor>> GetSigningCredentialsDescriptorAsync();
    }
}
