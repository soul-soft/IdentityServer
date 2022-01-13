using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ISecretsListParser
    {
        IEnumerable<string> GetAuthenticationMethods();
        Task<ClientSecret?> ParseAsync(HttpContext context);
    }
}
