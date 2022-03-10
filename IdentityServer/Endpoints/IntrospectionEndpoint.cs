using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class IntrospectionEndpoint : EndpointBase
    {
       
        public IntrospectionEndpoint()
        {
          
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
           throw new NotImplementedException();
        }
    }
}
