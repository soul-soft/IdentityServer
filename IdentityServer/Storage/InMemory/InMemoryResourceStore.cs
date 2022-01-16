﻿namespace IdentityServer.Storage
{
    internal class InMemoryResourceStore : IResourceStore
    {
        private readonly Resources _resources;

        public InMemoryResourceStore(Resources resources)
        {
            _resources = resources;
        }

        public Task<Resources> FindByScopeAsync(IEnumerable<string> scopes)
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

            var resources = new Resources(identityResources, apiScopes, apiResources);
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
