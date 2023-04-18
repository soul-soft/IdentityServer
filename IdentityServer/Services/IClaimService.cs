using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimService
    {
        Task<ClaimsPrincipal> GetTokenClaimsAsync(TokenGeneratorRequest request);
        Task<ClaimsPrincipal> GetProfileClaimsAsync(ProfileClaimsRequest request);
    }
}
