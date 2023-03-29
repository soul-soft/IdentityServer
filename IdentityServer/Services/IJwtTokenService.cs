namespace IdentityServer.Services
{
    public interface IJwtTokenService
    {
        Task<string> CreateJwtTokenAsync(Token token, IEnumerable<string> algorithms);
    }
}
