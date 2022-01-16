using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityServer.Models;

namespace IdentityServer.Serialization
{
    public class AccessTokenJsonConverter : JsonConverter<IAccessToken>
    {
        private readonly static JsonSerializerOptions _options;

        static AccessTokenJsonConverter()
        {
            _options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public override IAccessToken? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<AccessToken>(ref reader, _options);
        }

        public override void Write(Utf8JsonWriter writer, IAccessToken value, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Serialize(value, _options);
            writer.WriteRawValue(json);
        }
    }
}
