using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface IClientSecretValidator
    {
        Task<ClientSecretValidationResult> ValidateAsync(HttpContext context);
    }
}
