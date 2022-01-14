using System.Security.Claims;

namespace IdentityServer.Models
{
    public class TokenRequest
    {
        public string? Nonce { get; }
        public IClient Client { get; }
        public Resources Resources { get; }
        public string? SessionId { get; set; }
        public string? Description { get; set; }
        public ClaimsPrincipal Subject { get; }
        public string? GrantType { get; set; }
        public IEnumerable<string> Scopes { get; set; } = new HashSet<string>();
        public TokenRequest(ClaimsPrincipal subject, IClient client, Resources resources)
        {
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
