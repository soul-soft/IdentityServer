using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface ITokenValidator
    {
        Task<IEnumerable<Claim>> ValidateAsync(string? token);
    }
}
