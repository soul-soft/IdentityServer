using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ISecretParser
    {
        string AuthenticationMethod { get; }
        Task<ClientSecret?> ParseAsync(HttpContext context);
    }
}
