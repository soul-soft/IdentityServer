namespace IdentityServer.Models
{
    public interface ISecret
    {
        string? Description { get; }
        string Value { get; }
        DateTime? Expiration { get; }
        bool Enabled { get; }
        string? Type { get; }
    }
}
