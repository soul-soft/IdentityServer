using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Application
{
    public class Token
    {
        public DateTime? Expires { get; set; }
        public AccessTokenType AccessTokenType { get; set; }
        public HashSet<string> Audiences { get; set; } = new HashSet<string>();
        public HashSet<Claim> Claims { get; set; } = new HashSet<Claim>();
        public DateTime CreationTime { get; set; }
        public string Type { get; }
        public string Issuer { get; }
        public Token(string type, string issuer)
        {
            Type = type;
            Issuer = issuer;
        }
    }
}
