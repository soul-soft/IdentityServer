namespace IdentityServer.Models
{
    public class Secret
    {
        public string Value { get; set; } = null!;

        public DateTime? Expiration { get ; set; }

        public string? Description { get; set; }

        public Secret()
        {

        }

        public Secret(string value)
        {
            Value = value;
        }

        public Secret(string value, DateTime? expiration, string? description)
        {
            Value = value;
            Expiration = expiration;
            Description = description;
        }
    }
}
