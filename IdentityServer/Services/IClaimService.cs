using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimService
    {
        Task<ClaimsPrincipal> SignClaimsAsync(SingInAuthenticationContext context);
    }
}
