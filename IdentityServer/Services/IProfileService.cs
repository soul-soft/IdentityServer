using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<bool> IsActiveAsync(IsActiveContext context);
        Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequestContext context);
    }
}
