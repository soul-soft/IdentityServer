using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.ResponseGenerators
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly ISigningCredentialStore _credentials;

        public DiscoveryResponseGenerator(
            ISigningCredentialStore credentials)
        {
            _credentials = credentials;
        }

        public Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer)
        {
            var configuration = new OpenIdConnectConfiguration();
            configuration.Issuer = issuer;
            configuration.JwksUri = issuer + Constants.EndpointRoutePaths.DiscoveryJwks;
            configuration.AuthorizationEndpoint = issuer + Constants.EndpointRoutePaths.Authorize;
            configuration.TokenEndpoint = issuer + Constants.EndpointRoutePaths.Token;
            configuration.UserInfoEndpoint = issuer + Constants.EndpointRoutePaths.UserInfo;
            var response = new DiscoveryResponse(configuration);
            return Task.FromResult(response);
        }

        public async Task<JwkDiscoveryResponse> CreateJwkDiscoveryDocumentAsync()
        {
            var jwks = await _credentials.GetJsonWebKeysAsync();
            return new JwkDiscoveryResponse(jwks);
        }
    }
}
