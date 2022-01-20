using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Hosting
{
    internal static class EndpointExtensions
    {
        public static bool IsIdentityEndpoint(this Endpoint endpoint)
        {
            if (endpoint.Metadata.Any(a => a.GetType() == typeof(IdentityServerEndpoint)))
            {
                return true;
            }
            return false;
        }
    }
}
