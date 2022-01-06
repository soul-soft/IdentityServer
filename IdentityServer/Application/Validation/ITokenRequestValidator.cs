using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ITokenRequestValidator
    {
        Task<ValidationResult> ValidateRequestAsync(HttpContext context);
    }
}
