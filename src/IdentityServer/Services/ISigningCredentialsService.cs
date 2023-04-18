using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    public interface ISigningCredentialsService
    {
        Task<IEnumerable<SecurityKey>> GetSecurityKeysAsync();
        Task<IEnumerable<JsonWebKey>> GetJsonWebKeysAsync();
        Task<SigningCredentials> FindByAlgorithmsAsync(IEnumerable<string> algorithms);
        Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync();
    }
}
