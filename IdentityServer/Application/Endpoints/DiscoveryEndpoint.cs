using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Hosting.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DiscoveryEndpoint : IEndpointHandler
    {
        private readonly IDiscoveryResponseGenerator _responseGenerator;

        public DiscoveryEndpoint(IDiscoveryResponseGenerator responseGenerator)
        {
            _responseGenerator = responseGenerator;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var issuerUrl = context.GetIdentityServerIssuerUri();
            var baseUrl = issuerUrl.EnsureTrailingSlash();
            var entities = await _responseGenerator.CreateDiscoveryDocumentAsync(baseUrl, issuerUrl);
            return new DiscoveryDocumentResult(entities);
        }
    }
}
