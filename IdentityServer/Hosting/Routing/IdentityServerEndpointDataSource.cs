using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace IdentityServer.Hosting
{
    internal class IdentityServerEndpointDataSource : EndpointDataSource
    {
        private readonly CancellationToken _token;

        private readonly IEnumerable<EndpointDescriptor> _descriptors;

        public IdentityServerEndpointDataSource(IEnumerable<EndpointDescriptor> descriptors)
        {
            _token = new CancellationToken();
            _descriptors = descriptors;
        }

        public override IReadOnlyList<Endpoint> Endpoints
        {
            get
            {
                return CreateEndpoints().ToList();
            }
        }

        public override IChangeToken GetChangeToken()
        {
            return new CancellationChangeToken(_token);
        }

        private IEnumerable<Endpoint> CreateEndpoints()
        {
            foreach (var item in _descriptors)
            {
                RequestDelegate requestDelegate = async (context) =>
                {
                    var handler = (IEndpointHandler)context.RequestServices
                            .GetRequiredService(item.Handler);
                    var result = await handler.ProcessAsync(context);
                    await result.ExecuteAsync(context);
                };
                var routePattern = RoutePatternFactory.Parse(item.Pattern);
                var builder = new RouteEndpointBuilder(requestDelegate, routePattern, 0);
                var metadatas = new EndpointMetadataCollection();
                yield return builder.Build();
            }
        }
    }
}
