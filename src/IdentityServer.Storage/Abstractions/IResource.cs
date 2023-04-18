namespace IdentityServer.Models
{
    public interface IResource
    {
        bool Enabled { get; }
        string Name { get; }
        string? DisplayName { get; }
        string? Description { get; }
        bool ShowInDiscoveryDocument { get; }
        ICollection<string> ClaimTypes { get; }
    }
}
