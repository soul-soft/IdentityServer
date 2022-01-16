namespace IdentityServer.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(IAccessToken token, int lifetime);
        Task DeleteAsync(IRefreshToken refreshToken);
        Task<IRefreshToken?> GetAsync(string id);
    }
}
