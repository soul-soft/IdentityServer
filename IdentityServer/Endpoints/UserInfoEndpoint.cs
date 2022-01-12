using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        public override Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
