using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface ITokenStore
    {
        Task SaveTokenAsync(Token token);
        Task RevomeTokenAsync(Token token);
        Task<Token?> FindTokenAsync(string code);
    }
}
