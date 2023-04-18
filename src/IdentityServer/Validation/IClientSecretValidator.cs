using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public interface IClientSecretValidator
    {
        Task<Client> ValidateAsync(HttpContext context);
    }
}
