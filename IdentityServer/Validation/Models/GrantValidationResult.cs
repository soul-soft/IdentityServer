using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public ClaimsPrincipal Subject { get; set; }

        public GrantValidationResult(ClaimsPrincipal subject)
        {
            Subject = subject;
        }
    }
}
