using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(TokenValidatedRequest request);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(TokenValidatedRequest request);
    }
}
