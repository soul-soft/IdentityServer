using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Serialization
{
    public static class ObjectSerializer
    {
        public readonly static JsonSerializerOptions JsonSerializerOptions;

        static ObjectSerializer()
        {
            JsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }
    }
}
