using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ClaimsValidator : IClaimsValidator
    {
        public Task ValidateAsync(ClaimsPrincipal subject, Resources resources)
        {
            var allowedClaimTypes = resources.UserClaims;
            foreach (var item in subject.Claims)
            {
                if (!allowedClaimTypes.Contains(item.Type))
                {
                    throw new InvalidGrantException(string.Format("Invalid claim: {0}", item.Type));
                }
            }
            return Task.CompletedTask;
        }
    }
}
