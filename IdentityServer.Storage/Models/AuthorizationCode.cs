using System.Security.Claims;
using System.Text.Json.Serialization;

namespace IdentityServer.Models
{
    public class AuthorizationCode
    {
        public string Id { get; set; }
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
        public int Lifetime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTime { get; }

        public AuthorizationCode(string id, int lifetime, IEnumerable<Claim> claims, DateTime creationTime)
        {
            Id = id;
            Lifetime = lifetime;
            Claims = claims;
            ExpirationTime = creationTime.AddSeconds(lifetime); ;
            CreationTime = creationTime;
        }
    }
}
