namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateAsync(IToken token);
    }
}
