using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Code { get; set; }

        public string Type { get; set; }

        public int Lifetime { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        public DateTime ExpirationTime { get; set; }

        public DateTime CreationTime { get; set; }

        public Token(string code, string type, int lifetime, ICollection<Claim> claims, DateTime creationTime)
        {
            Code = code;
            Type = type;
            Lifetime = lifetime;
            Claims = claims;
            ExpirationTime = creationTime.AddSeconds(lifetime);
            CreationTime = creationTime;
        }

        public string? GetClientId()
        {
            return Claims
                .Where(a => a.Type == JwtClaimTypes.ClientId)
                .FirstOrDefault()?.Value;
        }

        public string? GetIssuer()
        {
            return Claims
               .Where(a => a.Type == JwtClaimTypes.Issuer)
               .FirstOrDefault()?.Value;
        }
    }
}
