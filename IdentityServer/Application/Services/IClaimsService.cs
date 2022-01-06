using System.Security.Claims;

namespace IdentityServer.Application
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsRequest request);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsRequest request);
    }
}
