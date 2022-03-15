namespace IdentityServer.Models
{
    public class ParsedSecret
    {
        public string ClientId { get; }
        public object Credentials { get; }
        public string Type { get; }

        public ParsedSecret(string id, object credentials, string type)
        {
            ClientId = id;
            Type = type;
            Credentials = credentials;
        }
    }
}
