using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Infrastructure
{
    internal static class ObjectSerializer
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }
      
        public static byte[] SerializeToUtf8Bytes(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj, JsonSerializerOptions);
        }
    }
}
