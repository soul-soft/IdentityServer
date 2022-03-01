using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<IEnumerable<Claim>> GetClaimDataAsync(ClaimDataRequestContext context);
        Task<Dictionary<string, object?>> GetProfileDataAsync(ProfileDataRequestContext context);
    }
}
