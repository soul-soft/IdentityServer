using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface IAccessTokenValidator
    {
        Task<IEnumerable<Claim>> ValidateAsync(string? token);
    }
}
