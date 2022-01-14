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
            return GrantValidationResult.SuccessAsync("2",new Claim[] 
            {
                new Claim(JwtClaimTypes.Role,"admin")
            });
        }
    }
}
