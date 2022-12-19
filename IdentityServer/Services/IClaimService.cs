using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimService
    {
        Task<ClaimsPrincipal> SignClaimsInAsync(SingInAuthenticationContext context);
    }
}
