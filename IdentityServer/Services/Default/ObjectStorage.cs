using IdentityServer.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityServer.Services
{
    internal class ObjectStorage : IObjectStorage
    {
        private readonly IDistributedCache _distributedCache;

        public ObjectStorage(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task SaveAsync(string key, object value, TimeSpan expiration)
        {
            var json = ObjectSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                SlidingExpiration = expiration,
            });
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return ObjectSerializer.Deserialize<T>(json);
        }
    }
}
