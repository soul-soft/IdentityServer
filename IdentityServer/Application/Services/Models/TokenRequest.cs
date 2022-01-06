using System.Security.Claims;
using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class TokenRequest
    {
        public string Issuer { get; private set; }
        public IClient Client { get; private set; }
        public string? SessionId { get; set; }
        public IEnumerable<string> Scopes { get; set; } = new List<string>();
        public string Type { get; private set; }
        public HashSet<Claim> Claims { get; private set; } = new HashSet<Claim>();
        public int? Lifetime { get; private set; }
        public IEnumerable<string> UserClaims { get; set; } = new List<string>();
        public TokenRequest(IClient client, string issuer, string type)
        {
            Client = client;
            Issuer = issuer;
            Type = type;
        }
    }
}
