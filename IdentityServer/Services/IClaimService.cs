using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimService
    {
        Task<ClaimsPrincipal> GetProfileClaimsAsync(ProfileClaimsRequest context);
        Task<ClaimsPrincipal> GetAccessTokenClaimsAsync(ProfileClaimsRequest context);
    }
}
