using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<bool> IsActiveAsync(IsActiveRequest request);
        Task<IEnumerable<Claim>> GetUserInfoClaimsAsync(ProfileClaimsRequest request);
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ProfileClaimsRequest request);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ProfileClaimsRequest request);
    }
}
