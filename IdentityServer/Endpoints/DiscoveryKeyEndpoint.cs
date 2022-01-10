using IdentityServer.ResponseGenerators;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;
using IdentityServer.Configuration;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/draft-ietf-jose-json-web-key-31
    /// </summary>
    public class DiscoveryKeyEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly IDiscoveryResponseGenerator _generator;

        public DiscoveryKeyEndpoint(
            IdentityServerOptions options,
            IDiscoveryResponseGenerator generator)
        {
            _options = options;
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!_options.Endpoints.EnableJwtRequestUri)
            {
                return MethodNotAllowed();
            }

            var response = await _generator.CreateJwkDiscoveryDocumentAsync();

            return DiscoveryResult(response);
        }
    }
}
