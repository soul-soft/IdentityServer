using System.Security.Claims;

namespace IdentityServer.Extensions
{
    public static class ClaimExtensions
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
    }
}
