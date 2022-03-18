using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public ClaimsPrincipal Subject { get; }

        public GrantValidationResult(ClaimsPrincipal subject)
        {
            Subject = subject;
        }
    }
}
