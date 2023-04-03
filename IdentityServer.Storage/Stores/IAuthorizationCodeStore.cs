using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IAuthorizationCodeStore
    {
        Task StoreAuthorizationCodeAsync(AuthorizationCode code);
        Task<AuthorizationCode?> FindAuthorizationCodeAsync(string code);
        Task RevomeAuthorizationCodeAsync(AuthorizationCode code);
    }
}
