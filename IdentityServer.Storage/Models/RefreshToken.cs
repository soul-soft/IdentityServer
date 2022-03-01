namespace IdentityServer.Models
{
    public class RefreshToken
    {
        public string Id { get; }

        public Token Token { get; }

        public int Lifetime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);
       
        public DateTime CreationTime { get; }

        public RefreshToken(string id, Token token, int lifetime, DateTime creationTime)
        {
            Id = id;
            Token = token;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
