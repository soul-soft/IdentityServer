using IdentityModel.Jwk;

namespace IdentityServer.Application
{
    public interface IDiscoveryResponseGenerator
    {
        Task<Dictionary<string, object>> CreateDiscoveryDocumentAsync(string baseUrl, string issuerUri);
    }
}
