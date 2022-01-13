using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ClaimLite : IClaimLite
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public ClaimLite(string name, string value, string valueType)
        {
            Name = name;
            Value = value;
            ValueType = valueType;
        }
        public ClaimLite(string name, string value)
            : this(name, value, ClaimValueTypes.String)
        {

        }
    }
}
