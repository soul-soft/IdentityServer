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
            Subject = new ClaimsPrincipal(identity);
        }

        public GrantValidationResult(
            string subject,
            string authenticationMethod,
            IEnumerable<Claim>? claims = null,
            string identityProvider = LocalIdentityProvider)
            : this(subject, authenticationMethod, DateTime.UtcNow, claims, identityProvider)
        {

        }

        public GrantValidationResult(
           string subject,
           string authenticationMethod,
           DateTime authenticationTime,
           IEnumerable<Claim>? claims = null,
           string identityProvider = LocalIdentityProvider)
        {
            var identity = new ClaimsIdentity(authenticationMethod);
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, subject));
            var authTimestamp = new DateTimeOffset(authenticationTime).ToUnixTimeSeconds().ToString();
            identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, authTimestamp, ClaimValueTypes.Integer64));
            if (claims != null)
            {
                identity.AddClaims(claims);
            }
            identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, identityProvider));
            Subject = new ClaimsPrincipal(identity);
        }
    }
}
