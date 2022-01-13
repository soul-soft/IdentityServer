using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class MyExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "myGrant";

        public Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationContext context)
        {
            return GrantValidationResult.SuccessAsync("2");
        }
    }
}
