using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting.Routing
{
    public interface IEndpointRouter
    {
        IEndpointHandler? Find(HttpContext context);
    }
}
