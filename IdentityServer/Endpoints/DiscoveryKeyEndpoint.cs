using IdentityServer.ResponseGenerators;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/draft-ietf-jose-json-web-key-31
    /// </summary>
    public class DiscoveryKeyEndpoint : EndpointBase
    {
        private readonly IDiscoveryResponseGenerator _generator;

        public DiscoveryKeyEndpoint(
            IDiscoveryResponseGenerator generator)
        {
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {

            var response = await _generator.CreateJwkDiscoveryDocumentAsync();

            return DiscoveryResult(response);
        }
    }
}
