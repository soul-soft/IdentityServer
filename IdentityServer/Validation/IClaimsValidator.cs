using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface IClaimsValidator
    {
        Task ValidateAsync(IEnumerable<Claim> claims, IEnumerable<string> claimTypes);
    }
}
