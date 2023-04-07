using IdentityServer.EntityFramework.Entities;
using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.EntityFramework.Stores
{
    internal class TokenStore : ITokenStore
    {
        private readonly IdentityServerDbContext _context;

        public TokenStore(IdentityServerDbContext context)
        {
            _context = context;
        }

        public async Task<Token?> FindTokenAsync(string token)
        {
            return (await _context.Tokens
               .Where(a => a.Code == token)
               .FirstOrDefaultAsync())?.Cast();
        }

        public async Task RevomeTokenAsync(Token token)
        {
            var entity = _context.Tokens.Where(a => a.Code == token.Code).First();
            _context.Tokens.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveTokenAsync(Token token)
        {
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task SetExpirationAsync(Token token)
        {
            var entity = _context.Tokens.Where(a => a.Code == token.Code).First();
            entity.ExpirationTime = token.ExpirationTime;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
