using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Authentication
{
    internal class OAuth2IntrospectionPostConfigureOptions : IPostConfigureOptions<OAuth2IntrospectionOptions>
    {
        public void PostConfigure(string name, OAuth2IntrospectionOptions options)
        {
            if (options.ConfigurationManager == null)
            {
                if (options.Configuration != null)
                {
                    options.ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(options.Configuration);
                }
                else if (!(string.IsNullOrEmpty(options.MetadataAddress) && string.IsNullOrEmpty(options.Authority)))
                {
                    if (string.IsNullOrEmpty(options.MetadataAddress) && !string.IsNullOrEmpty(options.Authority))
                    {
                        options.MetadataAddress = options.Authority;
                        if (!options.MetadataAddress.EndsWith("/", StringComparison.Ordinal))
                        {
                            options.MetadataAddress += "/";
                        }

                        options.MetadataAddress += ".well-known/openid-configuration";
                    }

                    if (options.RequireHttpsMetadata && !options.MetadataAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("The MetadataAddress or Authority must use HTTPS unless disabled for development by setting RequireHttpsMetadata=false.");
                    }

                    if (options.Backchannel == null)
                    {
                        options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                        options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("IdentityServer OAuth2Introspection handler");
                        options.Backchannel.Timeout = options.BackchannelTimeout;
                        options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
                    }

                    options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(options.MetadataAddress, new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever(options.Backchannel) { RequireHttps = options.RequireHttpsMetadata })
                    {
                        RefreshInterval = options.RefreshInterval,
                        AutomaticRefreshInterval = options.AutomaticRefreshInterval,
                    };
                }
            }
        }
    }
}
