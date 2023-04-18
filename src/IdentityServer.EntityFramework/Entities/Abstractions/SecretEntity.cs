using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class SecretEntity
    {
        public string Value { get; set; } = null!;
        public DateTime? Expiration { get; set; }

        public string? Description { get; set; }

        protected SecretEntity() { }

        public SecretEntity(string value, DateTime? expiration, string? description)
        {
            Value = value;
            Expiration = expiration;
            Description = description;
        }
    }
}
