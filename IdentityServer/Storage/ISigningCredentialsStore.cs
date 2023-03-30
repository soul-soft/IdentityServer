using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Storage
{
    public interface ISigningCredentialsStore
    {
        Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync();
    }
}
