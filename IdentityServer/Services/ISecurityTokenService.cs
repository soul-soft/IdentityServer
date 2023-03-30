namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateJwtTokenAsync(Token token, IEnumerable<string> algorithms);
    }
}
