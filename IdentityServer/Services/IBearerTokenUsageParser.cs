using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface IBearerTokenUsageParser
    {
        Task<string?> ParserAsync(HttpContext context);
    }
}
