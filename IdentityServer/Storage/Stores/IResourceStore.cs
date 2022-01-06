using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<IResource>> FindResourcesByScopesAsync(IEnumerable<string> scopes);
    }
}
