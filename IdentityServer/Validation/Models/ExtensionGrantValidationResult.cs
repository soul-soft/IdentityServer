using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationResult : GrantValidationResult
    {
        public ExtensionGrantValidationResult(string subject)
          : this(subject, Array.Empty<Claim>())
        {

        }

        public ExtensionGrantValidationResult(string subject, IEnumerable<Claim> claims)
        : this(subject, claims, DateTime.UtcNow)
        {

        }

        public ExtensionGrantValidationResult(string subject, IEnumerable<Claim> claims, DateTime authenticationTime, string authenticationMethod = GrantTypes.RefreshToken, string identityProvider = "local")
          : base(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(claims)
          {
              new Claim(JwtClaimTypes.Subject, subject),
              new Claim(JwtClaimTypes.AuthenticationTime, new DateTimeOffset(authenticationTime).ToUnixTimeSeconds().ToString()),
              new Claim(JwtClaimTypes.AuthenticationMethod, authenticationMethod),
              new Claim(JwtClaimTypes.IdentityProvider, identityProvider),
          }, GrantTypes.RefreshToken)))
        {

        }
    }
}
