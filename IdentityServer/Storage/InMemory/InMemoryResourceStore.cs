using IdentityServer.Models;

namespace IdentityServer.Storage.InMemory
{
    public class InMemoryResourceStore : IResourceStore
    {
        private readonly IEnumerable<Resource> _resources;

        public InMemoryResourceStore(IEnumerable<Resource> resources)
        {
            _resources = resources;
        }

        public Task<IEnumerable<Resource>> FindResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var query = from resource in _resources
                        where resource.Scopes.Any(name => scopeNames.Contains(name))
                        select resource;

            return Task.FromResult(query);
        }
    }
}
