using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public AuthorizationCode? Code { get; }

        public ClaimsPrincipal Subject { get; }

        public Dictionary<string, object> Properties { get; } = new();

        public GrantValidationResult()
        {
            Subject = new ClaimsPrincipal();
        }

        public GrantValidationResult(ClaimsPrincipal subject)
        {
            Subject = subject;
        }

        public GrantValidationResult(ClaimsPrincipal subject, AuthorizationCode? code = null)
        {
            Subject = subject;
            Code = code;
        }

        public GrantValidationResult(string subject, string? authenticationType = null, IEnumerable<Claim>? claims = null, AuthorizationCode? code = null)
        {
            var list = new List<Claim>();
            if (claims != null)
            {
                list.AddRange(claims);
            }
            list.Add(new Claim(JwtClaimTypes.Subject, subject));
            Subject = new ClaimsPrincipal(new ClaimsIdentity(list, authenticationType));
            Code = code;
        }
    }
}
