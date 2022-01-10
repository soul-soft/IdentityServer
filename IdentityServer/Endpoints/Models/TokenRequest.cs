using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class TokenRequest
    {
        public IClient Client { get; }
        public Resources Resources { get; }
        public TokenRequest(IClient client, Resources resources)
        {
            Client = client;
            Resources = resources;
        }
    }
}
