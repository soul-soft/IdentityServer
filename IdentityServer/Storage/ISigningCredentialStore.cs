using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        Task<IEnumerable<SecurityKey>> GetSecurityKeysAsync();
        Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync();
        Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(IEnumerable<string> algorithms);
        Task<IEnumerable<SigningCredentialsDescriptor>> GetSigningCredentialsAsync();
    }
}
