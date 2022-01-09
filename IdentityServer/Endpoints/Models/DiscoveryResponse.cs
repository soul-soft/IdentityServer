using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class DiscoveryResponse
    {
        public OpenIdConnectConfiguration Configuration { get; }

        public DiscoveryResponse(OpenIdConnectConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Serialize()
        {
            return OpenIdConnectConfiguration.Write(Configuration);
        }
    }
}
