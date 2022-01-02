namespace IdentityServer.Models
{
    public class Secret
    {
        public string? Description { get; }
        public string Credential { get; }
        public DateTime? Expiration { get; }
        public bool Enabled { get; } = true;
        public string? Type { get; }
        public Secret(string value)
        {
            Credential = value;
        }
    }
}
