using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        IEnumerable<SecurityKey> GetSecurityKeys();
        Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync();
        Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(IEnumerable<string> algorithms);
        Task<IEnumerable<SigningCredentialsDescriptor>> GetSigningCredentialsDescriptorAsync();
    }
}
