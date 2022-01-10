namespace IdentityServer.Models
{
    public class TokenCreationRequest
    {
        public string? Nonce { get; }
        public IClient Client { get; }
        public Resources Resources { get; }
        public string? SessionId { get; set; }
        public string? Description { get; set; }
        public string? SubjectId { get; set; }
        public List<string> Scopes { get; set; } = new List<string>();
        public TokenCreationRequest(IClient client, Resources resources)
        {
            Client = client;
            Resources = resources;
        }
    }
}
