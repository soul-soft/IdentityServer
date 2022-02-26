namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateJwtTokenAsync(AccessToken token);
    }
}
