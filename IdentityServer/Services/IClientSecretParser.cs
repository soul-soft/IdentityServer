using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface IClientSecretParser
    {
        string AuthenticationMethod { get; }
        Task<ClientSecret> ParseAsync(HttpContext context);
    }
}
