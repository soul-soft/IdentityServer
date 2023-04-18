using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimService
    {
        Task<ClaimsPrincipal> GetProfileClaimsAsync(ProfileClaimsRequest request);
        Task<ClaimsPrincipal> GetAccessTokenClaimsAsync(TokenGeneratorRequest request);
        Task<ClaimsPrincipal> GetIdentityTokenClaimsAsync(TokenGeneratorRequest request);
    }
}
