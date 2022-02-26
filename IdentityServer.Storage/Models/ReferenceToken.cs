namespace IdentityServer.Models
{
    public class ReferenceToken
    {
        public string Id { get; }

        public AccessToken AccessToken { get; }

        public int Lifetime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);

        public DateTime CreationTime { get; }

        public ReferenceToken(string id, AccessToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            AccessToken = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
