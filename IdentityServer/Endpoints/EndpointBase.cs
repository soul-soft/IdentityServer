using Microsoft.AspNetCore.Http;
using System.Net;

namespace IdentityServer.Endpoints
{
    public abstract class EndpointBase : IEndpointHandler
    {
        public abstract Task<IEndpointResult> HandleAsync(HttpContext context);

        protected static IEndpointResult DiscoveryEndpointResult(DiscoveryGeneratorResponse response)
        {
            return new DiscoveryResult(response);
        }

        protected static IEndpointResult JwkDiscoveryEndpointResult(JwkDiscoveryGeneratorResponse response)
        {
            return new JwkDiscoveryResult(response);
        }

        protected static IEndpointResult AuthorizeEndpointResult(AuthorizeGeneratorResponse response)
        {
            return new AuthorizeResult(response);
        }

        protected static IEndpointResult TokenEndpointResult(TokenGeneratorResponse response)
        {
            return new TokenResult(response);
        }

        protected static IEndpointResult IntrospectionResult(IntrospectionGeneratorResponse response)
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
