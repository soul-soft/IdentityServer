using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityServer.Models;

namespace IdentityServer.Serialization
{
    public class ReferenceTokenJsonConverter : JsonConverter<IReferenceToken>
    {
        private readonly static JsonSerializerOptions _options;

        static ReferenceTokenJsonConverter()
        {
            _options = new JsonSerializerOptions()
             {
                 DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                 DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
             };
            _options.Converters.Add(new AccessTokenJsonConverter());
        }

        public override IReferenceToken? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<ReferenceToken>(ref reader,_options);
        }

        public override void Write(Utf8JsonWriter writer, IReferenceToken value, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Serialize(value, _options);
            writer.WriteRawValue(json);
        }
    }
}
