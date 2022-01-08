using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class DiscoveryResponse
    {
        public OpenIdConnectConfiguration Configuration { get; set; } = null!;
    }
}
