namespace IdentityServer.Models
{
    public class RefreshToken : IRefreshToken
    {
        public string Id { get; }

        public IAccessToken AccessToken { get; }

        public int Lifetime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);
       
        public DateTime CreationTime { get; }

        public RefreshToken(string id, IAccessToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            AccessToken = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
