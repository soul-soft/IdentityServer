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

        public T? Deserialize<T>(byte[] bytes)
        {
            var span = new ReadOnlySpan<byte>(bytes);
            return JsonSerializer.Deserialize<T>(span, _serializerOptions);
        }

        public async Task SaveAsync(string key, object value, TimeSpan expiration)
        {
            var values = SerializeToUtf8Bytes(value);
            await _distributedCache.SetAsync(key, values, new DistributedCacheEntryOptions
            {
                SlidingExpiration = expiration,
            });
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var bytes = await _distributedCache.GetAsync(key);
            if (bytes == null)
            {
                return default;
            }
            return Deserialize<T>(bytes);
        }
    }
}
