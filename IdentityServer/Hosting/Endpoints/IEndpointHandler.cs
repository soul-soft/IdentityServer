using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public interface IEndpointHandler
    {
        Task<IEndpointResult> HandleAsync(HttpContext context);
    }
}
