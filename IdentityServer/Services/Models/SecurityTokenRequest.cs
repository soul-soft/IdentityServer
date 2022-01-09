using System.Security.Claims;
using IdentityServer.Protocols;

namespace IdentityServer.Models
{
    public class SecurityTokenRequest
    {
        public string? AllowedSigningAlgorithm { get; }
        public OpenIdConnectTokenType TokenType { get; set; } = OpenIdConnectTokenType.AccessToken;
        public IReadOnlyCollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}
