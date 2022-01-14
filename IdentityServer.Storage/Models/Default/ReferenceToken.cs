namespace IdentityServer.Models
{
    public class ReferenceToken : IReferenceToken
    {
        public string Id { get; }

        public IToken Token { get; }

        public int Lifetime { get; }

        public DateTime CreationTime { get; }

        public DateTime Expiration => CreationTime.AddSeconds(Lifetime);

        public ReferenceToken(string id, IToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            Token = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
