using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        Task<IEnumerable<SecurityKey>> GetSigningKeysAsync();
        Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync();
        Task<SigningCredentials> GetSigningCredentialsByAlgorithmsAsync(string? algorithm);
    }
}
