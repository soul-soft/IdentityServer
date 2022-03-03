using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface ITokenStore
    {
        Task StoreTokenAsync(Token token);
        Task<Token?> FindTokenAsync(string id);
    }
}
