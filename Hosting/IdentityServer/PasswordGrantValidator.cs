using IdentityServer.Models;
using IdentityServer.Validation;

namespace Hosting.Configuration
{
    public class PasswordGrantValidator : IPasswordGrantValidator
    {
        public Task ValidateAsync(PasswordGrantValidationRequest context)
        {
            if (context.Username == "test" && context.Password == "test")
            {
                return Task.CompletedTask;
            }
            throw new ValidationException(OpenIdConnectErrors.InvalidGrant, "用户名或密码错误");
        }
    }
}
