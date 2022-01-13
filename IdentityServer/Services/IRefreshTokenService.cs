namespace IdentityServer.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(IToken token, int lifetime);
        Task DeleteRefreshTokenAsync(IRefreshToken refreshToken);
        Task<IRefreshToken?> FindRefreshTokenAsync(string id);
    }
}
