using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace IdentityServer.Hosting
{
    internal class IdentityServerEndpointDataSource : EndpointDataSource
    {
        private List<Endpoint>? _endpoints;
        private CancellationTokenSource? _cts;
        private CancellationChangeToken? _cct;
        private readonly object Lock = new();

        private readonly IdentityServerOptions _options;

        private readonly EndpointDescriptorCollectionProvider _provider;

        public IdentityServerEndpointDataSource(
            IdentityServerOptions options,
            EndpointDescriptorCollectionProvider provider)
        {
            _options = options;
            _provider = provider;
        }

        public override IReadOnlyList<Endpoint> Endpoints
        {
            get
            {
                Initialize();
                return _endpoints!;
            }
        }

        public override IChangeToken GetChangeToken()
        {
            return _cct!;
        }

        private void Initialize()
        {
            if (_endpoints == null)
            {
                lock (Lock)
                {
                    if (_endpoints == null)
                    {
                        UpdateEndpoints();
                    }
                }
            }
        }

        private IEnumerable<Endpoint> CreateEndpoints()
        {
            foreach (var item in _provider)
            {
                async Task requestDelegate(HttpContext context)
                {
                    var handler = (IEndpointHandler)context.RequestServices
                            .GetRequiredService(item.Handler);
                    var result = await handler.ProcessAsync(context);
                    await result.ExecuteAsync(context);
                }
                var routePattern = RoutePatternFactory.Parse(item.RoutePattern);
                var builder = new RouteEndpointBuilder(requestDelegate, routePattern, 0);
                if (item.Name == Constants.EndpointNames.UserInfo)
                {
                    builder.Metadata.Add(new AuthorizeAttribute(_options.AuthorizationPolicyName));
                }
                builder.Metadata.Add(new IdentityServerEndpoint());

                yield return builder.Build();
            }
        }

        private void UpdateEndpoints()
        {
            lock (Lock)
            {
                var endpoints = CreateEndpoints();
                // Step 1 - capture old token
                var oldCts = _cts;
                // Step 2 - update endpoints
                _endpoints = endpoints.ToList();
                // Step 3 - create new change token
                _cts = new CancellationTokenSource();
                _cct = new CancellationChangeToken(_cts.Token);
                // Step 4 - trigger old token
                oldCts?.Cancel();
            }
        }
    }
}
