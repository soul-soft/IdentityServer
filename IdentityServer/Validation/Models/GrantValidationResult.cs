using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public IEnumerable<Claim> Claims { get; }

        public Dictionary<string, object> Properties { get; } = new();

        public GrantValidationResult()
        {
            Claims = new List<Claim>();
        }

        public GrantValidationResult(IEnumerable<Claim> claims)
        {
            Claims = claims;
        }

        public GrantValidationResult(string subject, IEnumerable<Claim>? claims = null)
            : this(new List<Claim>(claims ?? new Claim[0])
            {
                new Claim(JwtClaimTypes.Subject,subject)
            })
        {
        }
    }
}
