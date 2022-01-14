using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Serialization
{
    public class ClaimJsonConverter : JsonConverter<Claim>
    {
        public override Claim? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<Claim>(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, Claim value, JsonSerializerOptions options)
        {
            var data = new Dictionary<string, object>();
            data.Add("type", value.Type);
            data.Add("value", value.Value);
            data.Add("value", value.ValueType);
            var json = JsonSerializer.Serialize(data);
            writer.WriteRawValue(json);
        }
    }
}
