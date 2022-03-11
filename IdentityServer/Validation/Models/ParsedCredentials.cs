namespace IdentityServer.Models
{
    public class ParsedCredentials
    {
        public string ClientId { get; }
        public object Credentials { get; }
        public string Type { get; }

        public ParsedCredentials(string id, object credentials, string type)
        {
            ClientId = id;
            Type = type;
            Credentials = credentials;
        }
    }
}
