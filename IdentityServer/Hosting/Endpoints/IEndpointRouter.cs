using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public interface IEndpointRouter
    {
        IEndpointHandler? Routing(HttpContext context);
    }
}
