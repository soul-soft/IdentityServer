using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting.Routing
{
    public interface IEndpointHandler
    {
        Task<IEndpointResult> ProcessAsync(HttpContext context);
    }
}
