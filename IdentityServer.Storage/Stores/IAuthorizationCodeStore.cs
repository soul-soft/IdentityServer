using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IAuthorizationCodeStore
    {
        Task SaveAuthorizationCodeAsync(AuthorizationCode code);
        Task<AuthorizationCode?> FindAuthorizationCodeAsync(string code);
        Task RevomeAuthorizationCodeAsync(AuthorizationCode code);
    }
}
