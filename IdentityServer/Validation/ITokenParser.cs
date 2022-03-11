using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public interface ITokenParser
    {
        Task<string?> ParserAsync(HttpContext context);
    }
}
