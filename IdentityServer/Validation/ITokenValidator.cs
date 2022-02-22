using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface ITokenValidator
    {
        Task<ClaimsPrincipal> ValidateAsync(string? token);
    }
}
