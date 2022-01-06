using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ITokenEndpointGenerator
    {
        Task<ParsedCredential> GetCredentialAsync(HttpContext context);
        Task<IClient?> GetClientAsync(ParsedCredential credential);
    }
}
