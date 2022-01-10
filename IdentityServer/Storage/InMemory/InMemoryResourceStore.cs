using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Storage
{
    internal class InMemoryResourceStore : IResourceStore
    {
        private readonly IEnumerable<IResource> _resources;

        public InMemoryResourceStore(IEnumerable<IResource> resources)
        {
            _resources = resources;
        }
    }
}
