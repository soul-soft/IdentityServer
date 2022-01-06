using System.Security.Claims;

namespace IdentityServer.Application
{
    public interface IProfileService
    {
        Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequestRequest request);
    }
}
