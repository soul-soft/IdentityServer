using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationResult : GrantValidationResult
    {
        public ExtensionGrantValidationResult(string subject, string authenticationMethod)
            : this(subject, Array.Empty<Claim>(), authenticationMethod)
        {

        }

        public ExtensionGrantValidationResult(IEnumerable<Claim> claims, string authenticationMethod)
            : base(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(claims), authenticationMethod)))
        {

        }

        public ExtensionGrantValidationResult(string subject, IEnumerable<Claim> claims, string authenticationMethod)
            : this(subject, claims, DateTime.UtcNow, authenticationMethod)
        {

        }

        public ExtensionGrantValidationResult(string subject, IEnumerable<Claim> claims, DateTime authenticationTime, string authenticationMethod, string identityProvider = "local")
            : base(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(claims)
            {
                new Claim(JwtClaimTypes.Subject, subject),
                new Claim(JwtClaimTypes.AuthenticationTime, new DateTimeOffset(authenticationTime).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.AuthenticationMethod, authenticationMethod),
                new Claim(JwtClaimTypes.IdentityProvider, identityProvider),
            }, authenticationMethod)))
        {

        }
    }
}
