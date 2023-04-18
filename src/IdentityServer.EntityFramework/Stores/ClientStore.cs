using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EntityFramework.Stores
{
    internal class ClientStore : IClientStore
    {
        private readonly IdentityServerDbContext _context;

        public ClientStore(IdentityServerDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> FindClientAsync(string clientId)
        {
            return (await _context.Clients
                .Where(a => a.ClientId == clientId)
                .Where(a => a.Enabled)
                .Include(a => a.Secrets)
                .Include(a => a.Properties)
                .Include(a => a.AllowedScopes)
                .Include(a => a.AllowedGrantTypes)
                .Include(a => a.AllowedRedirectUris)
                .Include(a => a.AllowedSigningAlgorithms)
                .FirstOrDefaultAsync())?.Cast();
        }
    }
}
