using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class RefreshTokenValidationResult : GrantValidationResult
    {
        public RefreshTokenValidationResult(IEnumerable<Claim> claims)
            : base(new ClaimsPrincipal(new ClaimsIdentity(claims, GrantTypes.RefreshToken)))
        {

        }
    }
}
