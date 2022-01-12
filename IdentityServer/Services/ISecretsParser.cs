using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ISecretsParser
    {
        IEnumerable<string> GetAuthenticationMethods();
        Task<ClientSecret?> ParseAsync(HttpContext context);
    }
}
