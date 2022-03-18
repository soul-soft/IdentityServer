using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ResourceOwnerCredentialValidationResult : GrantValidationResult
    {
        public ResourceOwnerCredentialValidationResult(string subject)
          : this(subject, Array.Empty<Claim>())
        {

        }

        public ResourceOwnerCredentialValidationResult(string subject, IEnumerable<Claim> claims)
            : this(subject, claims, DateTime.UtcNow)
        {

        }

        public ResourceOwnerCredentialValidationResult(string subject, IEnumerable<Claim> claims, DateTime authenticationTime, string identityProvider = "local")
            : base(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(claims)
            {
                new Claim(JwtClaimTypes.Subject, subject),
                new Claim(JwtClaimTypes.AuthenticationTime, new DateTimeOffset(authenticationTime).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.AuthenticationMethod, GrantTypes.Password),
                new Claim(JwtClaimTypes.IdentityProvider, identityProvider),
            }, GrantTypes.Password)))
        {

        }
    }
}
