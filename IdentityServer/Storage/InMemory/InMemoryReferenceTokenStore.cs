using Microsoft.Extensions.Caching.Distributed;

namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IDistributedCache _cache;

        public InMemoryReferenceTokenStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SaveAsync(IReferenceToken token)
        {
            var key = CreateKey(token);
            var json = Serialize(token);
            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(token.Lifetime)
            });
        }

        private string CreateKey(IReferenceToken token)
        {
            return $"{Constants.IdentityServerName}:ReferenceToken:{token.Id}";
        }

        private string Serialize(IReferenceToken token)
        {
            return ObjectSerializer.SerializeObject(token);
        }
    }
}
