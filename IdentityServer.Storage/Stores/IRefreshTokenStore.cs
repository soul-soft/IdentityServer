using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IRefreshTokenStore
    {
        Task SaveAsync(IRefreshToken token);
        Task RevomeAsync(IRefreshToken token);
        Task<IRefreshToken?> FindRefreshTokenByIdAsync(string id);
    }
}
