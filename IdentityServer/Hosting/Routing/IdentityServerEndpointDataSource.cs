using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;

namespace IdentityServer.Hosting
{
    internal class IdentityServerEndpointDataSource : EndpointDataSource
    {
        private readonly object _lock = new object();
        private List<Endpoint>? _endpoints;
        private IChangeToken? _changeToken;
        private CancellationTokenSource? _cancellationTokenSource;

        private readonly IEnumerable<EndpointDescriptor> _descriptors;

        public IdentityServerEndpointDataSource(IEnumerable<EndpointDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }

        public override IReadOnlyList<Endpoint> Endpoints
        {
            get
            {
                Initialize();
                Debug.Assert(_endpoints != null);
                Debug.Assert(_changeToken != null);
                return _endpoints;
            }
        }
      
        public override IChangeToken GetChangeToken()
        {
            Debug.Assert(_endpoints != null);
            Debug.Assert(_changeToken != null);
            return _changeToken;
        }

        private void Initialize()
        {
            if (_endpoints == null)
            {
                lock (_lock)
                {
                    if (_endpoints == null)
                    {
                        UpdateEndpoints();
                    }
                }
            }
        }

        private void UpdateEndpoints()
        {
            lock (_lock)
            {
                var endpoints = CreateEndpoints().ToList();

                // See comments in DefaultActionDescriptorCollectionProvider. These steps are done
                // in a specific order to ensure callers always see a consistent state.

                // Step 1 - capture old token
                var oldCancellationTokenSource = _cancellationTokenSource;

                // Step 2 - update endpoints
                _endpoints = endpoints;

                // Step 3 - create new change token
                _cancellationTokenSource = new CancellationTokenSource();
                _changeToken = new CancellationChangeToken(_cancellationTokenSource.Token);

                // Step 4 - trigger old token
                oldCancellationTokenSource?.Cancel();
            }
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
                var routePattern = RoutePatternFactory.Parse(item.RoutePattern);
                var builder = new RouteEndpointBuilder(requestDelegate, routePattern, 0);
                yield return builder.Build();
            }
        }
    }
}
