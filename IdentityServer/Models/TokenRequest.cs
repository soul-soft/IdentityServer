using System.Security.Claims;

namespace IdentityServer.Models
{
    public class TokenRequest
    {
        public string AllowedSigningAlgorithm { get; }
        public IReadOnlyCollection<Claim> Claims { get; set; }
    }
}
