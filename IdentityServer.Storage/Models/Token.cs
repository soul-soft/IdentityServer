using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Code { get; set; } = default!;

        public string Type { get; set; } = default!;

        public int Lifetime { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        public DateTime ExpirationTime { get; set; }

        public DateTime CreationTime { get; set; }
        
        protected Token()
        {

        }

        public Token(string code, string type, int lifetime, ICollection<Claim> claims,DateTime expirationTime, DateTime creationTime)
        {
            Code = code;
            Type = type;
            Lifetime = lifetime;
            Claims = claims;
            ExpirationTime = expirationTime;
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
