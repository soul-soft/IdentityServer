using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Infrastructure
{
    internal static class ObjectSerializer
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyFields = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static string SerializeObject(object obj)
        {
            return JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }
    }
}
