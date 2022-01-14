using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IBearerTokenUsageParser _bearerTokenUsageParser;

        public UserInfoEndpoint()
        {
        }

        public override Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
           throw new NotImplementedException();
        }
    }
}
