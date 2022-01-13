namespace IdentityServer.Models
{
    public interface IReferenceToken
    {
        string Id { get; }
        IToken Token { get; }
        int Lifetime { get; }
        DateTime CreationTime { get; }
    }
}
