using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface IClaimsValidator
    {
        Task ValidateAsync(ClaimsPrincipal subject, Resources resources);
    }
}
