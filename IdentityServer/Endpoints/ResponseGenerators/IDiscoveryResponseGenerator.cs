namespace IdentityServer.Endpoints
{
    public interface IDiscoveryResponseGenerator
    {
        Task<JwkDiscoveryResponse> CreateJwkDiscoveryDocumentAsync();
        Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer, string baseUrl);
    }
}
