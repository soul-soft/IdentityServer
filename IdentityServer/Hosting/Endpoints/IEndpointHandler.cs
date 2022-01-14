using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public interface IEndpointHandler
    {
        Task<IEndpointResult> ProcessAsync(HttpContext context);
    }
}
