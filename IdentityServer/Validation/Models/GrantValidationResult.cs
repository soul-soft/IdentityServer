using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public ClaimsPrincipal Subject { get; } = new ClaimsPrincipal();

        public GrantValidationResult()
        {

        }

        public GrantValidationResult(ClaimsIdentity identity)
        {
            Subject.AddIdentity(identity);
        }

        public GrantValidationResult(string subject)
           : this(new ClaimsIdentity(new Claim[] { new Claim(JwtClaimTypes.Subject, subject) }))
        {

        }
    }
}
