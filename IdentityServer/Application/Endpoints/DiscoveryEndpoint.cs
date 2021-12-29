using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Hosting.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DiscoveryEndpoint : IEndpointHandler
    {
        protected IdentityServerOptions Options { get; }
        protected ILogger Logger { get; }

        public DiscoveryEndpoint(
            IdentityServerOptions options,
            ILogger<DiscoveryEndpoint> logger
            )
        {
            Options = options;
            Logger = logger;
        }
        public Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var entries = new Dictionary<string, object>();
            var issuer = context.GetIdentityServerIssuerUri();
            var baseUrl = issuer.EnsureTrailingSlash();
            entries.Add("issuer", issuer);
            if (Options.Discovery.ShowKeySet)
            {
                entries.Add(OidcConstants.Discovery.JwksUri, baseUrl + Constants.ProtocolRoutePaths.DiscoveryWebKeys);
            }
            // endpoints
            if (Options.Discovery.ShowEndpoints)
            {
                if (Options.Endpoints.EnableTokenEndpoint)
                {
                    entries.Add(OidcConstants.Discovery.TokenEndpoint, baseUrl + Constants.ProtocolRoutePaths.Token);
                }
            }
            if (Options.Endpoints.EnableAuthorizeEndpoint)
            {
                entries.Add(OidcConstants.Discovery.RequestParameterSupported, true);

                if (Options.Endpoints.EnableJwtRequestUri)
                {
                    entries.Add(OidcConstants.Discovery.RequestUriParameterSupported, true);
                }
            }
            // custom entries
            if (Options.Discovery.CustomEntries != null)
            {
                foreach ((string key, object value) in Options.Discovery.CustomEntries)
                {
                    if (entries.ContainsKey(key))
                    {
                        Logger.LogError("Discovery custom entry {key} cannot be added, because it already exists.", key);
                    }
                    else
                    {
                        if (value is string customValueString)
                        {
                            if (customValueString.StartsWith("~/") && Options.Discovery.ExpandRelativePathsInCustomEntries)
                            {
                                entries.Add(key, baseUrl + customValueString.Substring(2));
                                continue;
                            }
                        }

                        entries.Add(key, value);
                    }
                }
            }
            var result = new DiscoveryDocumentResult(entries);
            return Task.FromResult<IEndpointResult>(result);
        }
    }
}
