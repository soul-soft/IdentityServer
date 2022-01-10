namespace IdentityServer.Models
{
    public interface IApiResource : IResource
    {
        bool Required { get; }
        IReadOnlyCollection<string> Scopes { get; }
    }
}
