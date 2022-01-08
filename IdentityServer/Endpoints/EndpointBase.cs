using System.Net;
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

        protected Task<IEndpointResult> ResultAsync(HttpStatusCode statusCode)
        {
            var result = new StatusCodeResult(statusCode);
            return Task.FromResult<IEndpointResult>(result);
        }
    }
}
