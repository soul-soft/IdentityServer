using IdentityModel;
using IdentityModel.Jwk;
using IdentityServer.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DefaultDiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly ILogger Logger;
        private readonly IdentityServerOptions Options;
       
        public DefaultDiscoveryResponseGenerator(
            IdentityServerOptions options,
            ILogger<DefaultDiscoveryResponseGenerator> logger)
        {
            Options = options;
            Logger = logger;
        }
      
        public Task<Dictionary<string, object>> CreateDiscoveryDocumentAsync(string baseUrl, string issuerUri)
        {
            var entries = new Dictionary<string, object>();
            entries.Add("issuer", issuerUri);
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
            return Task.FromResult(entries);
        }

        public Task<IEnumerable<JsonWebKey>> CreateJwkDocumentAsync()
        {
            throw new NotImplementedException();
        }
    }
}
