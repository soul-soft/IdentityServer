namespace IdentityServer.Models
{
    public class RefreshToken : IRefreshToken
    {
        public string Id { get; }

        public IToken AccessToken { get; }

        public int Lifetime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);
       
        public DateTime CreationTime { get; }

        public RefreshToken(string id, IToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            AccessToken = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
