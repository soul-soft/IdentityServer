using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public static class ResourcesExtensions
    {
        public static Resources ToResources(this IEnumerable<IResource> resources)
        {
            return new Resources(resources);
        }
    }
}
