namespace IdentityServer.Models
{
    public interface IIdentityResource : IIdentityScope
    {
        bool Required { get; }
    }
}
