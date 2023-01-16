namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateTokenAsync(ReferenceToken token);
    }
}
