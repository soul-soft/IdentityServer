using IdentityServer.Hosting.Routing;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class DiscoveryKeyEndpoint : IEndpointHandler
    {
        public Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
