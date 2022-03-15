using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public interface IApiSecretValidator
    {
        Task<ApiResource> ValidateAsync(HttpContext context);
    }
}
