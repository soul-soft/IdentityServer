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
            return (await _context.AuthorizationCodes
                .Where(a => a.Code == code)
                .Include(a=>a.Claims)
                .FirstOrDefaultAsync())?.Cast();
        }

        public async Task RevomeAuthorizationCodeAsync(AuthorizationCode code)
        {
            var entity = _context.AuthorizationCodes.Where(a => a.Code == code.Code).First();
            _context.AuthorizationCodes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAuthorizationCodeAsync(AuthorizationCode code)
        {
            _context.AuthorizationCodes.Add(code);
            await _context.SaveChangesAsync();
        }
    }
}
