namespace IdentityServer.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(IToken token, int lifetime);
        Task<IRefreshToken?> FindRefreshTokenAsync(string id);
    }
}
