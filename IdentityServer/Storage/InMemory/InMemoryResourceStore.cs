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
                .Where(a => scopes.Contains(a.Name));

            var apiResources = _resources.ApiResources
                .Where(a => a.Scopes.Any(scope =>
                    scopes.Contains(scope)));

            var resources = new Resources(identityResources, apiResources);
            return Task.FromResult(resources);
        }
    }
}
