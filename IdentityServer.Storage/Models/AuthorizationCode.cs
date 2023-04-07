using System.Security.Claims;
using System.Text.Json.Serialization;

namespace IdentityServer.Models
{
    public class AuthorizationCode
    {
        public string Code { get; set; } = default!;
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public int Lifetime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreationTime { get; set; }
       
        protected AuthorizationCode()
        {

        }

        public AuthorizationCode(string code, int lifetime, ICollection<Claim> claims, DateTime creationTime)
        {
            Code = code;
            Lifetime = lifetime;
            Claims = claims;
            ExpirationTime = creationTime.AddSeconds(lifetime);
            CreationTime = creationTime;
        }
    }
}
