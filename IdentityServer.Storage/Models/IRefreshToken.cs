namespace IdentityServer.Models
{
    public interface IRefreshToken
    {
        string Id { get; }
        IAccessToken AccessToken { get; }
        int Lifetime { get; }
        DateTime Expiration { get; }
        DateTime CreationTime { get; }
    }
}
