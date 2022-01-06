using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialStore
    {
        Task<IEnumerable<SecurityKeyInfo>> GetSecurityKeysAsync();
        Task<SigningCredentials?> GetSigningCredentialsAsync();
        Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync();
    }
}
