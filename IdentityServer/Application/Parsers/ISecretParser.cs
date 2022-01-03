using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ISecretParser
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
        string AuthenticationMethod { get; }
    }
}
