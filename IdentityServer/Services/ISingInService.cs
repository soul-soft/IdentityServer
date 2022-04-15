using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ISingInService
    {
        Task<ClaimsPrincipal> SingInAsync(SingInAuthenticationContext context);
    }
}
