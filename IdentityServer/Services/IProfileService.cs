using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<bool> IsActiveAsync(IsActiveRequest request);
        Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest request);
    }
}
