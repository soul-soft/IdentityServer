using IdentityServer.Configuration;
using IdentityServer.Hosting;
using IdentityServer.Models;
using IdentityServer.ResponseGenerators;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly ITokenResponseGenerator _generator;

        public TokenEndpoint(
            IdentityServerOptions options,
            ITokenResponseGenerator generator)
        {
            _options = options;
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!_options.Endpoints.EnableTokenEndpoint)
            {
                return MethodNotAllowed();
            }
            var response = await _generator.ProcessAsync(new Client("", ""));
            return TokenResult(response);
        }
    }
}
