using System.Security.Claims;

namespace IdentityServer.Extensions
{
    public static class PrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(JwtClaimTypes.Subject);
            if (claim == null)
                throw new InvalidOperationException("sub claim is missing");
            return claim.Value;
        }
    }
}
