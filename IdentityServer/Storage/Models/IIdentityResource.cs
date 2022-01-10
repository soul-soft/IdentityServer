namespace IdentityServer.Models
{
    public interface IIdentityResource : IResource
    {
        bool Required { get; }
    }
}
