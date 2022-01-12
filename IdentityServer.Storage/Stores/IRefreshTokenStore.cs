using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IRefreshTokenStore
    {
        Task SaveAsync(IRefreshToken token);
    }
}
