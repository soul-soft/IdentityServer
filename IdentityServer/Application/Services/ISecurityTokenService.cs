namespace IdentityServer.Application
{
    public interface ISecurityTokenService
    {
        Task<string> CreateTokenAsync(Token token);
    }
}
