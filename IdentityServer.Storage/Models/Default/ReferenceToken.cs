namespace IdentityServer.Models
{
    public class ReferenceToken : IReferenceToken
    {
        public string Id { get; }

        public IToken AccessToken { get; }

        public int Lifetime { get; }

        public DateTime CreationTime { get; }

        public ReferenceToken(string id, IToken accessToken, int lifetime, DateTime creationTime)
        {
            Id = id;
            AccessToken = accessToken;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
