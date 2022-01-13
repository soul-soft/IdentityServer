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
        public string? SubjectId { get; set; }
        public string? GrantType { get;  set; }
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<string> Scopes { get; set; } = new HashSet<string>();

        public TokenRequest(IClient client, Resources resources)
        {
            Client = client;
            Resources = resources;
        }
    }
}
