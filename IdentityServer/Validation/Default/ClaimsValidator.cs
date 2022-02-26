using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class ClaimsValidator : IClaimsValidator
    {
        public Task ValidateAsync(IEnumerable<Claim> claims, IEnumerable<string> claimTypes)
        {
            foreach (var item in claims)
            {
                if (!claimTypes.Contains(item.Type))
                {
                    throw new InvalidGrantException(string.Format("Claim not granted: {0}", item.Type));
                }
            }
            return Task.CompletedTask;
        }
    }
}
