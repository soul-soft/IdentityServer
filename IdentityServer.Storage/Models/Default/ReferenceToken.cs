namespace IdentityServer.Models
{
    public class ReferenceToken : IReferenceToken
    {
        public string Id { get; }

        public IToken Token { get; }

        public int Lifetime { get; }

        public ReferenceToken(string id, IToken token)
        {
            Id = id;
            Token = token;
            Lifetime = Token.Lifetime;
        }
    }
}
