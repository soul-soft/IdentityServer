using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://openid.net/specs/openid-connect-discovery-1_0.html
    /// </summary>
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IIdentityServerUrl _urls;
        private readonly IDiscoveryResponseGenerator _generator;

        public DiscoveryEndpoint(
            IIdentityServerUrl urls,
            IDiscoveryResponseGenerator generator)
        {
            _urls = urls;
            _generator = generator;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            var response = await _generator.GetDiscoveryDocumentAsync();
            return DiscoveryEndpointResult(response);
        }

    }
}
