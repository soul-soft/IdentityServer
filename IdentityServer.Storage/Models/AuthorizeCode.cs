using System.Security.Claims;
using System.Text.Json.Serialization;

namespace IdentityServer.Models
{
    public class AuthorizeCode
    {
        public string Id { get; set; }
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
        public int Lifetime { get; }
        [JsonIgnore]
        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);
        [JsonIgnore]
        public string? SubjectId
        {
            get
            {
                return Claims.Where(s => s.Type == JwtClaimTypes.Subject)
                    .Select(s => s.Value).FirstOrDefault();
            }
        }
        public DateTime CreationTime { get; }

        public AuthorizeCode(string id, int lifetime,IEnumerable<Claim> claims)
        {
            Id = id;
            Lifetime = lifetime;
            Claims = claims;
        }
    }
}
