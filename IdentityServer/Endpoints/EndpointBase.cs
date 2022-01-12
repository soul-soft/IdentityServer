using System.Net;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public abstract class EndpointBase : IEndpointHandler
    {
        public abstract Task<IEndpointResult> ProcessAsync(HttpContext context);

        protected IEndpointResult DiscoveryResult(DiscoveryResponse response)
        {
            return new DiscoveryResult(response);
        }

        protected IEndpointResult DiscoveryResult(JwkDiscoveryResponse response)
        {
            return new JwkDiscoveryResult(response);
        }

        protected IEndpointResult TokenResult(TokenResponse response)
        {
            return new TokenResult(response);
        }

        protected IEndpointResult Result(HttpStatusCode statusCode)
        {
            return new StatusCodeResult(statusCode);
        }

        protected IEndpointResult BadRequest(string error, string? description = null)
        {
            return new ErrorResult(error, description, HttpStatusCode.BadRequest);
        }

        protected IEndpointResult MethodNotAllowed()
        {
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        protected IEndpointResult NotFound()
        {
            return new StatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}
