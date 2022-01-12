namespace IdentityServer.Models
{
    public class RefreshToken : IRefreshToken
    {
        public string Id { get; }

        public IToken Token { get; }

        public int Lifetime { get; }

        public RefreshToken(string id, IToken token, int lifetime)
        {
            Id = id;
            Token = token;
            Lifetime = lifetime;
        }
    }
}
