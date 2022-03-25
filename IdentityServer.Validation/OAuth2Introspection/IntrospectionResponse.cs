using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer.Validation
{
    internal class IntrospectionResponse
    {
        public JsonDocument Document { get; }

        private List<Claim> _claims = new List<Claim>();

        public IEnumerable<Claim> Claims => _claims;
       
        private bool _isActive = false;
        
        public bool IsActive => _isActive;
        
        public IntrospectionResponse(string json)
        {
            Document = JsonDocument.Parse(json);
            _isActive = Document.RootElement.GetProperty("active").GetBoolean();
            foreach (var property in Document.RootElement.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }
                else if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in property.Value.EnumerateArray())
                    {
                        _claims.Add(ConvertToClaim(property.Name, element));
                    }
                }
                else
                {
                    _claims.Add(ConvertToClaim(property.Name, property.Value));
                }
            }
        }

        private Claim ConvertToClaim(string name, JsonElement element)
        {
            var value = element.ToString();
            var valueType = ClaimValueTypes.String;
            switch (element.ValueKind)
            {
                case JsonValueKind.Undefined:
                    break;
                case JsonValueKind.Number:
                    valueType = ClaimValueTypes.Integer64;
                    break;
                case JsonValueKind.True:
                    valueType = ClaimValueTypes.Boolean;
                    break;
                case JsonValueKind.False:
                    valueType = ClaimValueTypes.Boolean;
                    break;
            }
            return new Claim(name, value, valueType);
        }
    }
}
