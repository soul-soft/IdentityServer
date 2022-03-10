using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Id { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Lifetime { get; set; }
        public string GrantType { get; set; } = null!;
        public string IdentityProvider { get; set; } = null!;
        public string? Issuer
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Issuer)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        public string? SubjectId
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Subject)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        public DateTime Expiration
        {
            get 
            {
                return CreationTime.AddSeconds(Lifetime);
            }
        }
        public ICollection<string> Audiences
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Audience)
                    .Select(s => s.Value).ToArray();
            }
        }
        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>();
        public AccessTokenType AccessTokenType { get; set; }
        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();
        public DateTime CreationTime { get; set; }
    }
}
