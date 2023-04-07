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

        public async Task<IEnumerable<ApiResource>> GetApiResourcesByNameAsync(string name)
        {
            return (await _context.ApiResources
                .Where(a => a.Enabled)
                .Where(a => a.Name == name)
                .ToListAsync())
                .Cast<ApiResource>();
        }

        public async Task<Resources> GetResourcesByScopesAsync(IEnumerable<string> scopes)
        {
            var identityResources = (await _context.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name))
                .ToListAsync())
                .Cast<IdentityResource>();

            var apiScopes = (await _context.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name))
                .ToListAsync())
                .Cast<ApiScope>();

            var apiScopeNames = apiScopes.Select(s => s.Name);

            var apiResources = (await _context.ApiResources
                .Where(a => a.Enabled)
                .Where(a => a.AllowedScopes.Any(a=> apiScopeNames.Contains(a.Value)))
                .ToListAsync())
                .Cast<ApiResource>();

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

