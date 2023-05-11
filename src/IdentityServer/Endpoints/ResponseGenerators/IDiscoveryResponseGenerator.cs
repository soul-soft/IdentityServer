namespace IdentityServer.Endpoints
{
    public interface IDiscoveryResponseGenerator
    {
        Task<DiscoveryGeneratorResponse> GetDiscoveryDocumentAsync();
        Task<JwkDiscoveryGeneratorResponse> CreateJwkDiscoveryDocumentAsync();
    }
}
