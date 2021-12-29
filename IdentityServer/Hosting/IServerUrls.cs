using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    public interface IServerUrls
    {
        public string Origin { get; }
    }
}
