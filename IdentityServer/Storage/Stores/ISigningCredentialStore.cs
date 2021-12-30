using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage.Stores
{
    public interface ISigningCredentialStore
    {
        Task<IEnumerable<SecurityKeyInfo>> GetSecurityKeysAsync();
        Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync();
    }
}
