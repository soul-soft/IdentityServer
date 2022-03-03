using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Id { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string GrantType { get; set; } = null!;
        public int Lifetime { get; set; }
        public AccessTokenType AccessTokenType { get; set; }
        public DateTime CreationTime { get; set; }
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
        public ICollection<string> Audiences
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Audience)
                    .Select(s => s.Value).ToArray();
            }
        }
        public ICollection<Claim> Claims { get; set; } = new HashSet<Claim>();
        public ICollection<string> AllowedSigningAlgorithms { get; set; } = new HashSet<string>();
        public DateTime Expiration
        {
            get 
            {
                return CreationTime.AddSeconds(Lifetime);
            }
        }
    }
}
