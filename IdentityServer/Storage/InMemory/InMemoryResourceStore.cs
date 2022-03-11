namespace IdentityServer.Storage
{
    internal class InMemoryResourceStore : IResourceStore
    {
        private readonly ResourceCollection _resources;

        public InMemoryResourceStore(ResourceCollection resources)
        {
            _resources = resources;
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(string name)
        {
            var resources =_resources.ApiResources
                .Where(a=>a.Enabled)
                .Where(a => a.Name == name);
           
            return Task.FromResult(resources);
        }

        public Task<ResourceCollection> FindResourcesByScopesAsync(IEnumerable<string> scopes)
        {
            var identityResources = _resources.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name));

            var apiResources = _resources.ApiResources
                .Where(a => a.Enabled)
                .Where(a => a.Scopes.Any(scope => scopes.Contains(scope)));

            var apiScopes = _resources.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name));

            var resources = new ResourceCollection(identityResources, apiScopes, apiResources);
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync()
        {
            var scopes = new List<string>();
            scopes.AddRange(_resources.IdentityResources
                .Where(a => a.Enabled)
                .Where(a=>a.ShowInDiscoveryDocument)
                .Select(s => s.Scope));
            scopes.AddRange(_resources.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => a.ShowInDiscoveryDocument)
                .Select(s => s.Scope));
            return Task.FromResult<IEnumerable<string>>(scopes);
        }
    }
}
