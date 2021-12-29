using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting.Routing
{
    public class EndpointRouter
        : IEndpointRouter
    {
        private readonly IEnumerable<Endpoint> _endpoints;

        public EndpointRouter(IEnumerable<Endpoint> endpoints)
        {
            _endpoints = endpoints;
        }

        public IEndpointHandler? Find(HttpContext context)
        {
            var endpoint = _endpoints
                .Where(a => a.Path == context.Request.Path)
                .FirstOrDefault();
            if (endpoint == null)
            {
                return null;
            }
            return GetEndpointHandler(context, endpoint);
        }

        private IEndpointHandler? GetEndpointHandler(HttpContext context, Endpoint endpoint)
        {
            return context.RequestServices
                .GetService(endpoint.Handler) as IEndpointHandler;
        }
    }
}
