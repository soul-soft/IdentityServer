using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ISignInService
    {
        Task<ClaimsPrincipal> SingInAsync(SingInAuthenticationContext context);
    }
}
