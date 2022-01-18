using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<IEnumerable<Claim>> GetProfileDataAsync(Resources resources);
    }
}
