namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateJwtTokenAsync(Client client, Token token);
    }
}
