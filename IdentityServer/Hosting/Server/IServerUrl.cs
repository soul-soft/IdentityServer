using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Hosting
{
    public interface IServerUrl
    {
        string GetIdentityServerIssuer();
    }
}
