using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface IClientCredentialsParser
    {
        string AuthenticationMethod { get; }
        Task<ClientCredentials> ParseAsync(HttpContext context);
    }
}
