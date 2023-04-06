namespace IdentityServer.EntityFramework.Entities
{
    public class ClaimEntity
    {
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string ValueType { get; set; } = null!;
        public string? Issuer { get; set; }
    }
}
