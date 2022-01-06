using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Application
{
    public class Token
    {
        public int? Lifetime { get; set; }
        public AccessTokenType AccessTokenType { get; set; }
        public HashSet<Claim> Claims { get; set; } = new HashSet<Claim>();
        public string Type { get; }
        public string Issuer { get; }
        public IEnumerable<string> Scopes { get; set; }
        public Token(string type, string issuer, IEnumerable<string> scopes)
        {
            Type = type;
            Issuer = issuer;
            Scopes = scopes;
        }
    }
}
