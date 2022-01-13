using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityServer.Models;

namespace IdentityServer.Serialization
{
    public class TokenJsonConverter : JsonConverter<IToken>
    {
        public override IToken? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<Token>(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, IToken value, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Serialize(value);
            writer.WriteRawValue(json);
        }
    }
}
