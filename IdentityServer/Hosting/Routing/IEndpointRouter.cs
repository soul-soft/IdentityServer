using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public interface IEndpointRouter
    {
        IEndpointHandler? Find(HttpContext context);
    }
}
