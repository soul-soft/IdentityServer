using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IProfileService
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync(UserClaimsProfileRequest request);
        Task<Dictionary<string, object?>> GetUserInfoAsync(UserInfoProfileRequest request);
    }
}
