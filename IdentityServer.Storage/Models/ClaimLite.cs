using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ClaimLite
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public ClaimLite(string type, string value, string valueType)
        {
            Type = type;
            Value = value;
            ValueType = valueType;
        }
        public ClaimLite(string type, string value)
            : this(type, value, ClaimValueTypes.String)
        {

        }
    }
}
