namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateAsync(IAccessToken token);
    }
}
