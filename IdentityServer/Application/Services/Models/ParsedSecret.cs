namespace IdentityServer.Application
{
    public class ParsedSecret
    {
        public string ClientId { get; }
        public string? Credential { get;  }
        public string Type { get; }
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public ParsedSecret(string clientId, string type)
        {
            ClientId = clientId;
            Type = type;
        }
       
        public ParsedSecret(string clientId, string secret, string type)
        {
            ClientId = clientId;
            Credential = secret;
            Type = type;
        }
    }
}
