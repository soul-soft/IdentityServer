using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class MyExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "myGrant";

        public Task ValidateAsync(ExtensionGrantValidationRequest context)
        {
            return Task.CompletedTask;
        }
    }
}
