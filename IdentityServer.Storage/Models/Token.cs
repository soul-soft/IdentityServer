using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Id { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Lifetime { get; set; }
        public AccessTokenType AccessTokenType { get; set; }
        public ClaimsPrincipal Subject { get; set; } = null!;
        public DateTime CreationTime { get; set; }
    }
}
