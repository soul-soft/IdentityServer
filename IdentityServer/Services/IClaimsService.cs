using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(TokenRequest request);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(TokenRequest request);
    }
}
