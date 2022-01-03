using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ITokenResponseGenerator
    {
        Task<TokenResponse> ProcessAsync(HttpContext context);
    }
}
