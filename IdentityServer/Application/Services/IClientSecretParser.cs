using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface IClientSecretParser
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
        IEnumerable<string> GetAvailableAuthenticationMethods();
    }
}
