namespace IdentityServer.Models
{
    public class ReferenceToken : IReferenceToken
    {
        public string Id { get; }

        public IAccessToken AccessToken { get; }

        public int Lifetime { get; }

        public DateTime CreationTime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);

        public ReferenceToken(string id, IAccessToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            AccessToken = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
