using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class DiscoveryKeyEndpoint : EndpointBase
    {
        public override Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
           throw new NotImplementedException();
        }
    }
}
