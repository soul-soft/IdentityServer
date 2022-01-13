namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateTokenAsync(IToken token);
    }
}
