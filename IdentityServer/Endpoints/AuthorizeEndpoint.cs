using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeEndpoint : EndpointBase
    {
        public override Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
