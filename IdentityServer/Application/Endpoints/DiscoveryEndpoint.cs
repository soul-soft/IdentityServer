using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class DiscoveryEndpoint : IEndpointHandler
    {
        private readonly IServerUrls _urls;
      
        private readonly IDiscoveryResponseGenerator _responseGenerator;

        public DiscoveryEndpoint(
            IServerUrls urls,
            IDiscoveryResponseGenerator responseGenerator)
        {
            _urls = urls;
            _responseGenerator = responseGenerator;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var issuerUrl = _urls.GetIdentityServerIssuerUri();
            var baseUrl = _urls.GetIdentityServerOrigin().EnsureTrailingSlash();
            var entities = await _responseGenerator.CreateDiscoveryDocumentAsync(baseUrl, issuerUrl);
            return new DiscoveryDocumentResult(entities);
        }
    }
}
