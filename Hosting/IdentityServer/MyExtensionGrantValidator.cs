using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class MyExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "myGrant";

        public Task ValidateAsync(ExtensionGrantRequestValidation context)
        {
            return Task.CompletedTask;
        }
    }
}
