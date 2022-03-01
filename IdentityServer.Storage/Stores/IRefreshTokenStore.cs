using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IRefreshTokenStore
    {
        Task StoreRefreshTokenAsync(RefreshToken token);
        Task RevomeRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> FindRefreshTokenAsync(string id);
    }
}
