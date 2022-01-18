using IdentityServer.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;

namespace IdentityServer.Hosting
{
    internal class IdentityServerEndpointDataSource : EndpointDataSource, IDisposable
    {
        private List<Endpoint>? _endpoints;
        private IChangeToken? _changeToken;
        private IDisposable? _disposable;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly object _lock = new object();
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

        public void Subscribe()
        {
            // IMPORTANT: this needs to be called by the derived class to avoid the fragile base class
            // problem. We can't call this in the base-class constuctor because it's too early.
            //
            // It's possible for someone to override the collection provider without providing
            // change notifications. If that's the case we won't process changes.
            _disposable = ChangeToken.OnChange(
                     () => _provider.GetChangeToken(),
                     UpdateEndpoints);
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
                    builder.Metadata.Add(new AuthorizeAttribute(_options.AuthenticationPolicyName));
                }
                yield return builder.Build();
            }
        }

        public void Dispose()
        {
            // Once disposed we won't process updates anymore, but we still allow access to the endpoints.
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
