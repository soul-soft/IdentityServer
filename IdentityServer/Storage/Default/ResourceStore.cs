namespace IdentityServer.Storage
{
    internal class ResourceStore : IResourceStore
    {
        private readonly Resources _resources;

        public ResourceStore(Resources resources)
        {
            _resources = resources;
        }

        public Task<IEnumerable<ApiResource>> GetApiResourcesByNameAsync(string name)
        {
            var resources = _resources.ApiResources
                .Where(a => a.Enabled)
                .Where(a => a.Name == name);

            return Task.FromResult(resources);
        }

        public Task<Resources> GetResourcesByScopesAsync(IEnumerable<string> scopes)
        {
            var identityResources = _resources.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name));

            var apiScopes = _resources.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name));

            var apiScopeNames = apiScopes.Select(s => s.Name);

            var apiResources = _resources.ApiResources
                .Where(a => a.Enabled)
                .Where(a => apiScopeNames.Contains(a.Scope));

            var resources = new Resources(identityResources, apiScopes, apiResources);
            return Task.FromResult(resources);
        }

        public Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync()
        {
            var scopes = new List<string>();
            scopes.AddRange(_resources.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => a.ShowInDiscoveryDocument)
                .Select(s => s.Scope));
            scopes.AddRange(_resources.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => a.ShowInDiscoveryDocument)
                .Select(s => s.Scope));
            return Task.FromResult<IEnumerable<string>>(scopes);
        }
    }
}
