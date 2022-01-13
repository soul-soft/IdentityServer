namespace IdentityServer.Models
{
    public class RefreshToken : IRefreshToken
    {
        public string Id { get; }

        public IToken Token { get; }

        public int Lifetime { get; }

        public DateTime CreationTime { get; }

        public RefreshToken(string id, IToken token, int lifetime, DateTime creationTime)
        {
            Id = id;
            Token = token;
            Lifetime = lifetime;
            CreationTime = creationTime;
        }
    }
}
