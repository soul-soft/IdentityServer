using System.Security.Claims;

namespace IdentityServer.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetClientId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(JwtClaimTypes.ClientId);
            return claim?.Value;
        }

        public static string? GetSubjectId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(JwtClaimTypes.Subject);
            return claim?.Value;
        }

        public static IEnumerable<string> GetScopes(this ClaimsPrincipal principal, bool emitScopesAsSpaceDelimitedStringInJwt)
        {
            if (emitScopesAsSpaceDelimitedStringInJwt)
            {
                return principal.FindFirstValue(JwtClaimTypes.Scope).Split(',');
            }
            return principal.FindAll(JwtClaimTypes.Scope).Select(s => s.Value);
        }
    }
}
