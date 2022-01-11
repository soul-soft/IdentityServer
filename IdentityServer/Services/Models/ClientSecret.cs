namespace IdentityServer.Models
{
    public class ClientSecret
    {
        public string Id { get; }
        public object? Credential { get; }
        public string Type { get; }

        public ClientSecret(string id, string type)
        {
            Id = id;
            Type = type;
        }

        public ClientSecret(string id, string type, object? credential)
        {
            Id = id;
            Type = type;
            Credential = credential;
        }
    }
}
