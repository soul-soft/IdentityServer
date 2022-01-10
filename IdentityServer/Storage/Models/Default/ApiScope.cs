using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class ApiScope : ResourceCollection, IApiScope
    {
        public ApiScope(string name) : base(name)
        {

        }
    }
}
