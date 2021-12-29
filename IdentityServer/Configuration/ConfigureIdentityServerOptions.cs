using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace IdentityServer.Configuration
{
    public class ConfigureIdentityServerOptions 
        : IPostConfigureOptions<IdentityServerOptions>
    {
        public void PostConfigure(string name, IdentityServerOptions options)
        {

        }
    }
}
