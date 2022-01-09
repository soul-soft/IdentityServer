using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public class EndpointRouter : IEndpointRouter
    {
        private readonly IEnumerable<EndpointDescriptor> _endpoints;

        public EndpointRouter(IEnumerable<EndpointDescriptor> endpoints)
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

        private IEndpointHandler? GetEndpointHandler(HttpContext context, EndpointDescriptor endpoint)
        {
            return context.RequestServices
                .GetService(endpoint.Handler) as IEndpointHandler;
        }
    }
}
