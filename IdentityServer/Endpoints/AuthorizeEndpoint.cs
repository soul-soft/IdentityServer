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
            throw new NotImplementedException();
        }
    }
}
