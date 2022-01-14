namespace IdentityServer.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(IToken token, int lifetime);
        Task DeleteAsync(IRefreshToken refreshToken);
        Task<IRefreshToken?> GetAsync(string id);
    }
}
