using System.Diagnostics;
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
                Debug.Assert(_endpoints != null);
                return _endpoints;
            }
        }

        public override IChangeToken GetChangeToken()
        {
            _cts = new CancellationTokenSource();
            return new CancellationChangeToken(_cts.Token);
        }

        private void Initialize()
        {
            if (_endpoints == null)
            {
                if (_endpoints == null)
                {
                    _endpoints = CreateEndpoints().ToList();
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

    }
}
