namespace IdentityServer.Models
{
    public interface IResource
    {
        string Name { get; }
        string? DisplayName { get; }
        string? Description { get; }
        string Scope { get; }
        IReadOnlyCollection<string> UserClaims { get; }
    }
}
