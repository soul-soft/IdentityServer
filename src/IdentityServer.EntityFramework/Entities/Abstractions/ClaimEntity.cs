using System.Security.Claims;

namespace IdentityServer.EntityFramework.Entities
{
    public class ClaimEntity
    {
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string ValueType { get; set; } = null!;
        public string? Issuer { get; set; }
        protected ClaimEntity()
        {

        }
        public ClaimEntity(string type, string value, string valueType, string? issuer)
        {
            Type = type;
            Value = value;
            ValueType = valueType;
            Issuer = issuer;
        }
    }
}
