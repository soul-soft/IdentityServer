using IdentityServer.Models;

namespace IdentityServer.Storage.InMemory
{
    public class InMemoryResourceStore : IResourceStore
    {
        private readonly IEnumerable<IResource> _resources;

        public InMemoryResourceStore(IEnumerable<IResource> resources)
        {
            _resources = resources;
        }

        public Task<IEnumerable<IResource>> FindResourcesByScopesAsync(IEnumerable<string> scopes)
        {
            var query = from resource in _resources
                        where scopes.Contains(resource.Scope)
                        select resource;
            return Task.FromResult(query);
        }
    }
}
