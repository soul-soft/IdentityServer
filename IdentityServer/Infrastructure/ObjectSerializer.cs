using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Infrastructure
{
    internal static class ObjectSerializer
    {
        readonly static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, SerializerOptions);
        }
    }
}
