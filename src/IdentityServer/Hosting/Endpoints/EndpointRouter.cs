using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Hosting
{
    public class EndpointRouter : IEndpointRouter
    {
        private readonly IdentityServerOptions _options;
        private readonly ILogger<EndpointRouter> _logger;
        private readonly IEnumerable<EndpointDescriptor> _endpoints;
       
        public EndpointRouter(
            IdentityServerOptions options,
            ILogger<EndpointRouter> logger,
            IEnumerable<EndpointDescriptor> endpoints)
        {
            _options = options;
            _logger = logger;
            _endpoints = endpoints;
        }

        public IEndpointHandler? Routing(HttpContext context)
        {
            foreach (var endpoint in _endpoints)
            {
                if (context.Request.Path.Equals(endpoint.Path, StringComparison.OrdinalIgnoreCase))
                {
                    if (!_options.Endpoints.IsEndpointEnabled(endpoint))
                    {
                        _logger.LogWarning("Endpoint disabled: {endpoint}", endpoint.Name);
                        break;
                    }
                    return context.RequestServices.GetRequiredService(endpoint.HandlerType) as IEndpointHandler;
                }
            }
            return null;
        }
    }
}
