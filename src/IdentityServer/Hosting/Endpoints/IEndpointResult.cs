using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public interface IEndpointResult
    {
        Task ExecuteAsync(HttpContext context);
    }
}
