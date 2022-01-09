using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.ResponseGenerators
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly ISigningCredentialStore _credentials;
        private readonly EndpointDescriptorCollection _endpoints;

        public DiscoveryResponseGenerator(
            ISigningCredentialStore credentials,
            EndpointDescriptorCollection endpoints)
        {
            _endpoints = endpoints;
            _credentials = credentials;
        }

        public Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer)
        {
            var configuration = new OpenIdConnectConfiguration();
            configuration.Issuer = issuer;
            configuration.JwksUri = issuer + _endpoints.DiscoveryJwks;
            configuration.AuthorizationEndpoint = issuer + _endpoints.Authorize;
            configuration.TokenEndpoint = issuer + _endpoints.Token;
            configuration.UserInfoEndpoint = issuer + _endpoints.UserInfo;
            var response = new DiscoveryResponse(configuration);
            return Task.FromResult(response);
        }

        public async Task<JwkDiscoveryResponse> CreateJwkDiscoveryDocumentAsync()
        {
            var signingKeys = await _credentials.GetSigningKeysAsync();
            return new JwkDiscoveryResponse(signingKeys);
        }
    }
}
