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
                .Include(a => a.ClaimTypes)
                .Include(a => a.Properties)
                .ToListAsync())
                .Cast<ApiResource>();
        }

        public async Task<Resources> GetResourcesByScopesAsync(IEnumerable<string> scopes)
        {
            var identityResources = (await _context.IdentityResources
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name))
                .Include(a => a.ClaimTypes)
                .ToListAsync())
                .Select(s => s.Cast());

            var apiScopes = (await _context.ApiScopes
                .Where(a => a.Enabled)
                .Where(a => scopes.Contains(a.Name))
                .Include(a => a.ClaimTypes)
                .ToListAsync())
                .Select(s => s.Cast());

            var apiScopeNames = apiScopes.Select(s => s.Name);

            var apiResources = (await _context.ApiResources
                .Where(a => a.Enabled)
                .Where(a => a.AllowedScopes.Any(a => apiScopeNames.Contains(a.Data)))
                .Include(a => a.ClaimTypes)
                .Include(a => a.Properties)
                .Include(a => a.AllowedScopes)
                .Include(a => a.Secrets)
                .ToListAsync())
                .Select(s => s.Cast());

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

