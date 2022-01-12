using Microsoft.Extensions.Caching.Distributed;
using IdentityServer.Infrastructure;
using IdentityServer.Models;
using IdentityServer.Protocols;

namespace IdentityServer.Storage
{
    internal class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IDistributedCache _cache;

        public InMemoryRefreshTokenStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SaveAsync(IRefreshToken token)
        {
            var key = CreateKey(token);
            var json = Serialize(token);
            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(token.Lifetime)
            });
        }

        private string CreateKey(IRefreshToken token)
        {
            return $"{Constants.IdentityServerName}:RefreshToken:{token.Id}";
        }

        private string Serialize(IRefreshToken token)
        {
            return ObjectSerializer.SerializeObject(token);
        }
    }
}
