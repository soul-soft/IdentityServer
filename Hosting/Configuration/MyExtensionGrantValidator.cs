using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class MyExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "myGrant";

        public Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationContext context)
        {
            var result = new GrantValidationResult("2");
            return Task.FromResult(result);
        }
    }
}
