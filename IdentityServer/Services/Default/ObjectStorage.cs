using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityServer.Services
{
    internal class ObjectStorage : IObjectStorage
    {
        private readonly IDistributedCache _distributedCache;

        private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        
        public ObjectStorage(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public byte[] SerializeToUtf8Bytes(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj, _serializerOptions);
        }

        public async Task SaveAsync(string key, object value, TimeSpan expiration)
        {
            var values = SerializeToUtf8Bytes(value);
            await _distributedCache.SetAsync(key, values, new DistributedCacheEntryOptions
            {
                SlidingExpiration = expiration,
            });
        }
    }
}
