using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://openid.net/specs/openid-connect-discovery-1_0.html
    /// </summary>
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IServerUrl _urls;
        private readonly IdentityServerOptions _options;
        private readonly IDiscoveryResponseGenerator _generator;

        public DiscoveryEndpoint(
            IServerUrl urls,
            IdentityServerOptions options,
            IDiscoveryResponseGenerator generator)
        {
            _urls = urls;
            _options = options;
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            var baseUrl = _urls.GetIdentityServerBaseUrl();
            var issuerUrl = _urls.GetIdentityServerIssuerUri();
            var response = await _generator.CreateDiscoveryDocumentAsync(issuerUrl, baseUrl);
            return DiscoveryEndpointResult(response);
        }

    }
}
