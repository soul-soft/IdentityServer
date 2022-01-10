using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Parsers
{
    public interface ISecretParse
    {
        Task<ParsedSecret?> ParseAsync(HttpContext context);
    }
}
