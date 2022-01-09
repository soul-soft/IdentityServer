using IdentityServer.Hosting;
using IdentityServer.Models;
using IdentityServer.ResponseGenerators;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly ITokenResponseGenerator _generator;

        public TokenEndpoint(ITokenResponseGenerator generator)
        {
            _generator = generator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var response = await _generator.ProcessAsync(new Client());
            return TokenResult(response);
        }
    }
}
