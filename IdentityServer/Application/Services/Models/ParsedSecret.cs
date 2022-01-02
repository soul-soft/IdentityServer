namespace IdentityServer.Application
{
    public class ParsedSecret
    {
        public string ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public ParsedSecret(string clientId, string type)
        {
            ClientId = clientId;
            Type = type;
        }
       
        public ParsedSecret(string clientId, string secret, string type)
        {
            ClientId = clientId;
            ClientSecret = secret;
            Type = type;
        }
    }
}
