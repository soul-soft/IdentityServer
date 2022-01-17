using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public string? Subject { get; }

        public IEnumerable<Claim> Claims { get; } = new List<Claim>();

        public GrantValidationResult()
        {

        }

        public GrantValidationResult(string subject)
        {
            Subject = subject;
        }

        public GrantValidationResult(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }

        public GrantValidationResult(string subject, IEnumerable<Claim> claims)
        {
            Subject = subject;
            Claims = claims;
        }
    }
}
