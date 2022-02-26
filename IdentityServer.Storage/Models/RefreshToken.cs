namespace IdentityServer.Models
{
    public class RefreshToken
    {
        public string Id { get; }

        public AccessToken AccessToken { get; }

        public int Lifetime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);
       
        public DateTime CreationTime { get; }

        public RefreshToken(string id, AccessToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            AccessToken = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
