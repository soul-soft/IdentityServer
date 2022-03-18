using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class MyExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "myGrant";

        public Task<ExtensionGrantValidationResult> ValidateAsync(ExtensionGrantValidationRequest request)
        {
            return Task.FromResult(new ExtensionGrantValidationResult("10", GrantType));
        }
    }
}
