using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Parsers
{
    public interface IUrlParse
    {
        string GetIdentityServerIssuerUri();
    }
}
