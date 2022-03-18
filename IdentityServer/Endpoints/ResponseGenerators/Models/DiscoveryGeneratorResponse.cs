using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class DiscoveryGeneratorResponse
    {
        public OpenIdConnectConfiguration Configuration { get; }

        public DiscoveryGeneratorResponse(OpenIdConnectConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Serialize()
        {
            return OpenIdConnectConfiguration.Write(Configuration);
        }
    }
}
