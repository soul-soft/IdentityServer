using IdentityServer.Models;
using IdentityServer.Validation;
using System.Security.Claims;

namespace Hosting.Configuration
{
    public class MyExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "myGrant";

        public Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationContext context)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, "1"));
            identity.AddClaim(new Claim(JwtClaimTypes.Role, "admin"));
            var result = new GrantValidationResult(identity.Claims);
            return Task.FromResult(result);
        }
    }
}
