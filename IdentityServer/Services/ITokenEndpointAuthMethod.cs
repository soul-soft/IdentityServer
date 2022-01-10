using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ITokenEndpointAuthMethod
    {
        string AuthMethod { get; }
        Task<ParsedClientSecret?> ParseAsync(HttpContext context);
    }
}
