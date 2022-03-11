using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public interface IApiSecretValidator
    {
        Task ValidateAsync(HttpContext context);
    }
}
