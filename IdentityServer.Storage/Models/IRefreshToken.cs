namespace IdentityServer.Models
{
    public interface IRefreshToken
    {
        string Id { get; }
        IToken Token { get; }
        int Lifetime { get; }
        DateTime CreationTime { get; }
    }
}
