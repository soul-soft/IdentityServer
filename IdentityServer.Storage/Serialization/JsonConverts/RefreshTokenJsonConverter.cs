using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityServer.Models;

namespace IdentityServer.Serialization
{
    public class RefreshTokenJsonConverter : JsonConverter<IRefreshToken>
    {
        public override IRefreshToken? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<RefreshToken>(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, IRefreshToken value, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Serialize(value);
            writer.WriteRawValue(json);
        }
    }
}
