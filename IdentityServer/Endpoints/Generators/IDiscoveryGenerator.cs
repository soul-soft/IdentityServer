namespace IdentityServer.Endpoints
{
    public interface IDiscoveryGenerator
    {
        Task<JwkDiscoveryResponse> CreateJwkDiscoveryDocumentAsync();
        Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer);
    }
}
