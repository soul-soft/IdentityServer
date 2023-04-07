using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EntityFramework.Stores
{
    internal class AuthorizationCodeStore : IAuthorizationCodeStore
    {
        private readonly IdentityServerDbContext _context;

        public AuthorizationCodeStore(IdentityServerDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorizationCode?> FindAuthorizationCodeAsync(string code)
        {
            return await _context.AuthorizationCodes
                .Where(a => a.Code == code)
                .FirstOrDefaultAsync();
        }

        public async Task RevomeAuthorizationCodeAsync(AuthorizationCode code)
        {
            _context.AuthorizationCodes.Remove(code);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAuthorizationCodeAsync(AuthorizationCode code)
        {
            _context.Entry(code).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
