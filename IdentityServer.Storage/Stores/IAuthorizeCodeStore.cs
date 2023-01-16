using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IAuthorizeCodeStore
    {
        Task StoreAuthorizeCodeAsync(AuthorizeCode code);
        Task<AuthorizeCode?> FindByAuthorizeCodeAsync(string id);
    }
}
