using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Hosting.Routing
{
    public class Endpoint
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Type Handler { get; set; }

        public Endpoint(string name, string path, Type handler)
        {
            Name = name;
            Path = path;
            Handler = handler;
        }
    }
}
