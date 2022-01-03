namespace IdentityServer.Models
{
    public class Secret : ISecret
    {
        public string? Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public bool Enabled { get; set; } = true;
        public string? Type { get; set; }
        public Secret(string value)
        {
            Value = value;
            Type = IdentityServerConstants.SecretTypes.SharedSecret;
        }
    }
}
