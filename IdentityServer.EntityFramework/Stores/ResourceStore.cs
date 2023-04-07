using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EntityFramework.Stores
{
    internal class ResourceStore : IResourceStore
    {
        private readonly IdentityServerDbContext _context;

        public ResourceStore(IdentityServerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesAsync(string name)
        {
            return await _context.ApiResources
                .Where(a => a.Enabled)
                .Where(a => a.Name == name)
                .ToListAsync();
        }

        public async Task<Resources> FindResourcesAsync(IEnumerable<string> scopes)
        {
            var identityResources = await _context.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name))
                .ToListAsync();

            var apiScopes = await _context.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name))
                .ToListAsync();

            var apiScopeNames = apiScopes.Select(s => s.Name);

            var apiResources = await _context.ApiResources
                .Where(a => a.Enabled)
                .Where(a => apiScopeNames.Contains(a.Scope))
                .ToListAsync();

            return new Resources(identityResources, apiScopes, apiResources);
        }

        public async Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync()
        {
            var scopes = new List<string>();
            var apiScopes = await _context.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => a.ShowInDiscoveryDocument)
                .Select(s => s.Scope)
                .ToListAsync();
            scopes.AddRange(apiScopes);
            var identityScopes = await _context.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => a.ShowInDiscoveryDocument)
                .Select(s => s.Scope)
                .ToListAsync();
            scopes.AddRange(identityScopes);
            return scopes;
        }
    }
}

