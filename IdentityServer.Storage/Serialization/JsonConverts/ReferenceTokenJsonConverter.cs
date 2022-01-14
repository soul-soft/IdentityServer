using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityServer.Models;

namespace IdentityServer.Serialization
{
    public class ReferenceTokenJsonConverter : JsonConverter<IReferenceToken>
    {
        public override IReferenceToken? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<ReferenceToken>(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, IReferenceToken value, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Serialize(value);
            writer.WriteRawValue(json);
        }
    }
}
