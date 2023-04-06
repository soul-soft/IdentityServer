namespace IdentityServer.EntityFramework.Entities
{
    public class SecretEntity
    {
        public DateTime? Expiration { get; set; }

        public string Value { get; set; } = null!;

        public string? Description { get; set; }
    }
}
