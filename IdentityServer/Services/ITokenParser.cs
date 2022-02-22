using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface ITokenParser
    {
        Task<string?> ParserAsync(HttpContext context);
    }
}
