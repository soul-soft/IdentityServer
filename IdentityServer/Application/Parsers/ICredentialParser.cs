using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ICredentialParser
    {
        Task<ParsedCredential> ParseAsync(HttpContext context);
        string AuthenticationMethod { get; }
    }
}
