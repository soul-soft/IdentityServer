using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    public class DiscoveryKeyEndpoint : EndpointBase
    {
        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var configuration = new OpenIdConnectConfiguration();
            configuration.Issuer = "";
            return await ResultAsync(new DiscoveryResponse()
            {
                Configuration = configuration,
            });
        }
    }
}
