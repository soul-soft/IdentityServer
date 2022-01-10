using IdentityServer.Endpoints;

namespace IdentityServer.ResponseGenerators
{
    public interface IDiscoveryResponseGenerator
    {
        Task<JwkDiscoveryResponse> CreateJwkDiscoveryDocumentAsync();
        Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer);
    }
}
