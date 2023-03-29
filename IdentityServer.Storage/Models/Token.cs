using System.Security.Claims;

namespace IdentityServer.Models
{
    public class Token
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public int Lifetime { get; set; }

        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();

        public DateTime CreationTime { get; set; }

        public Token(string id, string type, int lifetime, IEnumerable<Claim> claims, DateTime creationTime)
        {
            Id = id;
            Type = type;
            Lifetime = lifetime;
            AccessTokenType = accessTokenType;
            Claims = claims;
            CreationTime = creationTime;
        }
    }
}
