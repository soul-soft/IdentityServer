namespace IdentityServer.Services
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync(Token token);
    }
}
