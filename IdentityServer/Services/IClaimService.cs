using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimService
    {
        Task<ClaimsPrincipal> GetProfileClaimsAsync(ProfileClaimsRequest request);
        Task<ClaimsPrincipal> GetAccessTokenClaimsAsync(AccessTokenClaimsRequest request);
    }
}
