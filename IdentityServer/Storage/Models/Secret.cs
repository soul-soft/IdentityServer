namespace IdentityServer.Models
{
    public class Secret
    {
        public string? Description { get; }
        public string Value { get; }
        public DateTime? Expiration { get; }
        public bool Enabled { get; } = true;
        public string? Type { get; }
        public Secret(string value)
        {
            Value = value;
            Type = IdentityServerConstants.SecretTypes.SharedSecret;
        }
    }
}
