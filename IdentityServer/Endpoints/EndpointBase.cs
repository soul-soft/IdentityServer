using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public abstract class EndpointBase : IEndpointHandler
    {
        public abstract Task<IEndpointResult> ProcessAsync(HttpContext context);

        protected Task<IEndpointResult> ResultAsync(DiscoveryResponse response)
        {
            var result = new DiscoveryResult(response);
            return Task.FromResult<IEndpointResult>(result);
        }
    }
}
