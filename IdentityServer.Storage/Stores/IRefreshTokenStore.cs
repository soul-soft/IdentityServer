using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IRefreshTokenStore
    {
        Task AddAsync(RefreshToken token);
        Task RevomeAsync(RefreshToken token);
        Task<RefreshToken?> FindByIdAsync(string id);
    }
}
