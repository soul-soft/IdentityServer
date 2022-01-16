using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ValidatedTokenRequest request);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request);
    }
}
