using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface IClientSecretScheme
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
        string AuthenticationMethod { get; }
    }
}
