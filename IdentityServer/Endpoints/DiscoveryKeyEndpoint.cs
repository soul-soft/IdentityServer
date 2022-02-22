using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/draft-ietf-jose-json-web-key-31
    /// </summary>
    public class DiscoveryKeyEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly IDiscoveryGenerator _generator;

        public DiscoveryKeyEndpoint(
            IdentityServerOptions options,
            IDiscoveryGenerator generator)
        {
            _options = options;
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!_options.Endpoints.EnableDiscoveryJwksEndpoint)
            {
                return MethodNotAllowed();
            }

            var response = await _generator.CreateJwkDiscoveryDocumentAsync();

            return DiscoveryResult(response);
        }
    }
}
