using System.Security.Claims;
using IdentityServer.Protocols;

namespace IdentityServer.Models
{
    public class SecurityTokenRequest
    {
        public int? Lifetime { get; set; }
        public string Issuer { get; set; } = null!;
        public string? AllowedSigningAlgorithm { get; }
        public OpenIdConnectTokenType TokenType { get; set; } = OpenIdConnectTokenType.AccessToken;
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }
}
