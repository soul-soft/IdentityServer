using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface IScopeParser
    {
        Task<IEnumerable<string>> RequestScopeAsync(string scope);
        Task<IEnumerable<string>> ParseAsync(ClaimsPrincipal subject);
    }
}
