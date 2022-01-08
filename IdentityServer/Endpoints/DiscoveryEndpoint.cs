using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IServerUrl _urls;

        public DiscoveryEndpoint(IServerUrl urls)
        {
            _urls = urls;
        }

        public override Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var configuration = new OpenIdConnectConfiguration();
            configuration.Issuer = _urls.GetIdentityServerIssuer();
            return ResultAsync(new DiscoveryResponse()
            {
                Configuration = configuration
            });
        }
    }
}
