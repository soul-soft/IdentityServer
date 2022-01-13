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
                return GrantValidationResult.ResultAsync("1",new Claim[] 
                {
                    new Claim(JwtClaimTypes.Role,"admin")
                });
            }
            return GrantValidationResult.ErrorAsync("用户名或密码错误");
        }
    }
}
