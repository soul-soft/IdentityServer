using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateAccessTokenAsync(SecurityTokenRequest request);
    }
}
