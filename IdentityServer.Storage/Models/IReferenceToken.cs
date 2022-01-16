namespace IdentityServer.Models
{
    public interface IReferenceToken
    {
        string Id { get; }
        IAccessToken AccessToken { get; }
        int Lifetime { get; }
        DateTime Expiration { get; }
        DateTime CreationTime { get; }
    }
}
