using System.Security.Claims;
using IdentityServer.Protocols;

namespace IdentityServer.Models
{
    public class TokenRequest
    {
        public string? Nonce { get; }
        public IClient Client { get; }
        public Resources Resources { get; }
        public string? SessionId { get; set; }
        public string? Description { get; set; }
        public string? SubjectId { get; set; }
        public List<string> Scopes { get; set; } = new List<string>();
        public TokenRequest(IClient client, Resources resources)
        {
            Client = client;
            Resources = resources;
        }
    }
}
