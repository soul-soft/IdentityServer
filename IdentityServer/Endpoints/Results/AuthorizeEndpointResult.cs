using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class AuthorizeEndpointResult : IEndpointResult
    {
        private readonly AuthorizeGeneratorRequest _request;
       
        public AuthorizeEndpointResult(AuthorizeGeneratorRequest request)
        {
            _request = request;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var result = await context.AuthenticateAsync();
        }
    }
}
