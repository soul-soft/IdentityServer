using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface ITokenValidator
    {
        Task<ClaimsPrincipal> ValidateAccessTokenAsync(string? token);
    }
}
