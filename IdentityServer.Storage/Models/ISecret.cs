namespace IdentityServer.Models
{
    public interface ISecret
    {
        DateTime? Expiration { get; }
        string Value { get; }
        string? Description { get; }
    }
}
