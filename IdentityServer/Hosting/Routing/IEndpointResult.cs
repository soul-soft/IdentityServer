using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting.Routing
{
    public interface IEndpointResult
    {
        Task ExecuteAsync(HttpContext context);
    }
}
