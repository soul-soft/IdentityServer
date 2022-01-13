namespace IdentityServer.Models
{
    public interface IReferenceToken
    {
        string Id { get; }
        IToken AccessToken { get; }
        int Lifetime { get; }
        DateTime CreationTime { get; }
    }
}
