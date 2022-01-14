using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ISecretResolver
    {
        string AuthenticationMethod { get; }
        Task<ClientSecret?> ParseAsync(HttpContext context);
    }
}
