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
        private readonly IUrlParse _urls;
        private readonly IdentityServerOptions _options;
        private readonly IDiscoveryResponseGenerator _generator;

        public DiscoveryEndpoint(
            IUrlParse urls,
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
