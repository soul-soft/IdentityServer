using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class DiscoveryKeyEndpoint : IEndpointHandler
    {
        private readonly IDiscoveryKeyResponseGenerator _responseGenerator;

        public DiscoveryKeyEndpoint(IDiscoveryKeyResponseGenerator responseGenerator)
        {
            _responseGenerator = responseGenerator;
        }
        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var keys = await _responseGenerator.CreateJwkDocumentAsync();

            return new DiscoveryKeyResult(keys);
        }
    }
}
