using System.Text.Json.Serialization;

namespace IdentityServer.Models
{
    public class RefreshToken
    {
        public string Id { get; }

        public ReferenceToken Token { get; }

        public int Lifetime { get; }
        [JsonIgnore]
        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);
       
        public DateTime CreationTime { get; }

        public RefreshToken(string id, ReferenceToken token, int lifetime, DateTime creationTime)
        {
            Id = id;
            Token = token;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
