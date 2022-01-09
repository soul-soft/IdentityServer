using IdentityServer.ResponseGenerators;
using IdentityServer.Configuration;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://openid.net/specs/openid-connect-discovery-1_0.html
    /// </summary>
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IServerUrl _urls;
        private readonly IDiscoveryResponseGenerator _generator;
        private readonly IdentityServerOptions _options;

        public DiscoveryEndpoint(
            IServerUrl urls,
            IDiscoveryResponseGenerator generator,
            IdentityServerOptions options)
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
            if (!_options.Endpoints.EnableDiscoveryEndpoint)
            {
                return MethodNotAllowed();
            }
            var issuer = _urls.GetIdentityServerIssuer();
            var response = await _generator.CreateDiscoveryDocumentAsync(issuer);
            return DiscoveryResult(response);
        }

    }
}
