using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ClientCredentialsValidationResult : GrantValidationResult
    {
        public ClientCredentialsValidationResult(IEnumerable<Claim> claims)
           : base(new ClaimsPrincipal(new ClaimsIdentity(claims, GrantTypes.RefreshToken)))
        {

        }
    }
}
