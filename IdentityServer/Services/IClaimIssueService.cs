using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimIssueService
    {
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(TokenRequest request);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(TokenRequest request);
    }
}
