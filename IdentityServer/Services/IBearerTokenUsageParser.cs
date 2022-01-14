using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public interface IBearerTokenUsageParser
    {
        Task<string?> GetBearerTokenAsync(HttpContext context);
    }
}
