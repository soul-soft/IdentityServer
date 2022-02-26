using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://openid.net/specs/openid-connect-discovery-1_0.html
    /// </summary>
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IServerUrl _urls;
        private readonly IDiscoveryGenerator _generator;

        public DiscoveryEndpoint(
            IServerUrl urls,
            IDiscoveryGenerator generator)
        {
            _urls = urls;
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            var issuer = _urls.GetIssuerUri();
            var response = await _generator.CreateDiscoveryDocumentAsync(issuer);
            return DiscoveryEndpointResult(response);
        }

    }
}
