using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class AuthorizeCodeValidationResult : GrantValidationResult
    {
        public AuthorizeCodeValidationResult(ClaimsPrincipal claims)
            : base(claims)
        {

        }
    }
}
