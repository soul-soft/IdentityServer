using System.Net;
using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    /// <summary>
    /// https://openid.net/specs/openid-connect-discovery-1_0.html
    /// </summary>
    internal class DiscoveryEndpoint : EndpointBase
    {
        private readonly IServerUrl _urls;
        private readonly IdentityServerOptions _options;

        public DiscoveryEndpoint(IServerUrl urls, IdentityServerOptions options)
        {
            _urls = urls;
            _options = options;
        }

        public override Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return ResultAsync(HttpStatusCode.MethodNotAllowed);
            }
            if (!_options.Endpoints.EnableDiscoveryEndpoint)
            {
                return ResultAsync(HttpStatusCode.NotFound);
            }
            var issuer = _urls.GetIdentityServerIssuer();
            var document = CreateDiscoveryDocument(issuer);
            return ResultAsync(new DiscoveryResponse(document));
        }

        public OpenIdConnectConfiguration CreateDiscoveryDocument(string issuer)
        {
            var document = new OpenIdConnectConfiguration();
            document.Issuer = issuer;
            document.AuthorizationEndpoint = issuer + OpenIdConnectRoutePaths.Authorize;
            document.TokenEndpoint = issuer + OpenIdConnectRoutePaths.Token;
            document.UserInfoEndpoint = issuer + OpenIdConnectRoutePaths.UserInfo;
            document.JwksUri = issuer + OpenIdConnectRoutePaths.Jwks;
            return document;
        }
    }
}
