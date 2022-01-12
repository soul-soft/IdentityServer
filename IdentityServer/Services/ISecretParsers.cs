using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ISecretParsers
    {
        IEnumerable<string> GetAuthenticationMethods();
        Task<ClientSecret?> ParseAsync(HttpContext context);
    }
}
