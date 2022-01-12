namespace IdentityServer.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(IToken token, int lifetime);
    }
}
