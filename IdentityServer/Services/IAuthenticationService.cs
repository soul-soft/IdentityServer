using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IAuthenticationService
    {
        Task<ClaimsPrincipal> SingInAsync(AuthenticationSingInContext context);
    }
}
