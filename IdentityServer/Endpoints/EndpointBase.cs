using Microsoft.AspNetCore.Http;
using System.Net;

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

        protected IEndpointResult BadRequest()
        {
            return new StatusCodeResult(HttpStatusCode.BadRequest);
        }

        protected IEndpointResult Unauthorized()
        {
            return new StatusCodeResult(HttpStatusCode.Unauthorized);
        }

        protected IEndpointResult MethodNotAllowed()
        {
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }
    }
}
