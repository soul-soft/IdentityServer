namespace IdentityServer.Models
{
    public class ClientCredentials
    {
        public string ClientId { get; }
        public object Credentials { get; }
        public string Type { get; }

        public ClientCredentials(string id, object credentials, string type)
        {
            ClientId = id;
            Type = type;
            Credentials = credentials;
        }
    }
}
