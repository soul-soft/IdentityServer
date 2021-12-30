using IdentityServer.Models;

namespace IdentityServer.Application
{
    public interface IDiscoveryKeyResponseGenerator
    {
        Task<IEnumerable<JsonWebKey>> CreateJwkDocumentAsync();
    }
}
