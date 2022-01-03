using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ISecretListParser
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
        IEnumerable<string> GetAuthenticationMethods();
    }
}
