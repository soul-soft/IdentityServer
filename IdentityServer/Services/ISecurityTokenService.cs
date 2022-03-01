namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateTokenAsync(Token token);
    }
}
