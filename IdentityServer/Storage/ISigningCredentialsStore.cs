using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialsStore
    {
        Task<IEnumerable<SecurityKey>> GetSecurityKeysAsync();
        Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync();
        Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(IEnumerable<string> algorithms);
        Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync();
    }
}
