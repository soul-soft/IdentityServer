using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class DiscoveryResponse
    {
        public OpenIdConnectConfiguration Configuration { get; } = null!;

        public DiscoveryResponse(OpenIdConnectConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Write()
        {
            return OpenIdConnectConfiguration.Write(Configuration);
        }
    }
}
