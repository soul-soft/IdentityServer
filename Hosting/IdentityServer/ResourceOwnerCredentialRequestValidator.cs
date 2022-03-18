using IdentityServer.Models;
using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        public Task ValidateAsync(ResourceOwnerCredentialValidationRequest context)
        {
            if (context.Username == "test" && context.Password == "test")
            {
                return Task.CompletedTask;
            }
            throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "用户名或密码错误");
        }
    }
}
