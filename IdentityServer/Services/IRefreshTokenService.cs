namespace IdentityServer.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(AccessToken token, int lifetime);
        Task DeleteRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string id);
    }
}
