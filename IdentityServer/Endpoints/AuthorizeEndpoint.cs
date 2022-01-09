using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeEndpoint : EndpointBase
    {
        public override Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
