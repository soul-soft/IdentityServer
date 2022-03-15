using Microsoft.AspNetCore.Http;
using System.Net;

namespace IdentityServer.Endpoints
{
    public abstract class EndpointBase : IEndpointHandler
    {
        public abstract Task<IEndpointResult> ProcessAsync(HttpContext context);

        protected static IEndpointResult DiscoveryEndpointResult(DiscoveryResponse response)
        {
            return new DiscoveryResult(response);
        }

        protected static IEndpointResult JwkDiscoveryEndpointResult(JwkDiscoveryResponse response)
        {
            return new JwkDiscoveryResult(response);
        }

        protected static IEndpointResult TokenEndpointResult(TokenResponse response)
        {
            return new TokenResult(response);
        }

        protected static IEndpointResult IntrospectionResult(IntrospectionResponse response)
        {
            return new IntrospectionResult(response);
        }

        protected static IEndpointResult BadRequest(string error, string? errorDescription)
        {
            return new BadRequestResult(error, errorDescription);
        }

        protected static IEndpointResult Unauthorized(string error, string? errorDescription)
        {
            return new UnauthorizedResult(error, errorDescription);
        }

        protected static IEndpointResult MethodNotAllowed()
        {
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }
    }
}
