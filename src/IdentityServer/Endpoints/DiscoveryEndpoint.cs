using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://openid.net/specs/openid-connect-discovery-1_0.html
    /// </summary>
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IDiscoveryResponseGenerator _generator;

        public DiscoveryEndpoint(
            IDiscoveryResponseGenerator generator)
        {
            _generator = generator;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            var response = await _generator.GetDiscoveryDocumentAsync();
            return Json(response.Serialize());
        }
    }
}
