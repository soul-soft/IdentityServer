using IdentityServer.Models;
using IdentityServer.Validation;
using System.Security.Claims;

namespace Hosting.Configuration
{
    public class ResourceOwnerPasswordGrantValidator
        : IResourceOwnerPasswordGrantValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantValidationContext context)
        {
            if (context.Username == "test" && context.Password == "test")
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(JwtClaimTypes.Subject, "1"));
                identity.AddClaim(new Claim(JwtClaimTypes.Role, "admin"));
                var result = new GrantValidationResult(identity.Claims);
                return Task.FromResult(result);
            }
            throw new InvalidGrantException("用户名或密码错误");
        }
    }
}
