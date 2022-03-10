using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Storage.Serialization
{
    internal class ClaimJsonConverter : JsonConverter<Claim>
    {
        public override Claim? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var claimLite = JsonSerializer.Deserialize<ClaimLite>(ref reader, options);
            if (claimLite == null)
            {
                return null;
            }
            return new Claim(claimLite.Name, claimLite.Value, claimLite.Type);
        }

        public override void Write(Utf8JsonWriter writer, Claim value, JsonSerializerOptions options)
        {
            var claimLite = new ClaimLite(value.Type, value.Value, value.ValueType);
            JsonSerializer.Serialize(writer, claimLite, options);
        }
    }
}
