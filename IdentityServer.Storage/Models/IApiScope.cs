namespace IdentityServer.Models
{
    public interface IApiScope : IResource, IScope
    {
        bool Required { get; }
        bool Emphasize { get; }
    }
}
