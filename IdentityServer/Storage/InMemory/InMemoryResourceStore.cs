using IdentityServer.Models;

namespace IdentityServer.Storage
{
    internal class InMemoryResourceStore : IResourceStore
    {
        private readonly Resources _resources;

        public InMemoryResourceStore(Resources resources)
        {
            _resources = resources;
        }

        public Task<Resources> FindResourcesByScopeAsync(IEnumerable<string> scopes)
        {
            var identityResources = _resources.IdentityResources
                .Where(a => scopes.Contains(a.Name))
                .DistinctBy(a => a.Name);

            var apiResources = _resources.ApiResources
                .Where(a => a.Scopes.Any(scope => scopes.Contains(scope)))
                .DistinctBy(a => a.Name);

            var apiScopes = _resources.ApiScopes
                .Where(a => scopes.Contains(a.Name))
                .DistinctBy(a => a.Name);

            var resources = new Resources(identityResources, apiResources, apiScopes);
            return Task.FromResult(resources);
        }
    }
}
