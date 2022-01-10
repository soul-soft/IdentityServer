using IdentityServer.Configuration;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;

        public AuthorizeEndpoint(
            IdentityServerOptions options)
        {
            _options = options;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            await Task.CompletedTask;
            if (!_options.Endpoints.EnableAuthorizeEndpoint)
            {
                return MethodNotAllowed();
            }
            throw new NotImplementedException();
        }
    }
}
