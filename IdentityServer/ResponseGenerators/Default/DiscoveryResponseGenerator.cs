using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Services;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static IdentityServer.OpenIdConnects;

namespace IdentityServer.ResponseGenerators
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly IResourceStore _resources;
        private readonly ISecretParsers _secretParsers;
        private readonly ISigningCredentialStore _credentials;

        public DiscoveryResponseGenerator(
            IResourceStore resources,
            ISecretParsers secretParsers,
            ISigningCredentialStore credentials)
        {
            _resources = resources;
            _credentials = credentials;
            _secretParsers = secretParsers;
        }

        public async Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer)
        {
            var configuration = new OpenIdConnectConfiguration();
            configuration.Issuer = issuer;
            configuration.JwksUri = issuer + Constants.EndpointRoutePaths.DiscoveryJwks;
            configuration.AuthorizationEndpoint = issuer + Constants.EndpointRoutePaths.Authorize;
            configuration.TokenEndpoint = issuer + Constants.EndpointRoutePaths.Token;
            configuration.UserInfoEndpoint = issuer + Constants.EndpointRoutePaths.UserInfo;
            configuration.GrantTypesSupported.Add(GrantTypes.ClientCredentials);
            configuration.GrantTypesSupported.Add(GrantTypes.Password);
            configuration.GrantTypesSupported.Add(GrantTypes.AuthorizationCode);
            configuration.GrantTypesSupported.Add(GrantTypes.RefreshToken);
            var scopes = await _resources.GetScopesAsync();
            foreach (var item in scopes)
            {
                configuration.ScopesSupported.Add(item);
            }
            foreach (var item in _secretParsers.GetAuthenticationMethods())
            {
                configuration.TokenEndpointAuthMethodsSupported.Add(item);
            }
            var response = new DiscoveryResponse(configuration);
            return response;
        }

        public async Task<JwkDiscoveryResponse> CreateJwkDiscoveryDocumentAsync()
        {
            var jwks = await _credentials.GetJsonWebKeysAsync();
            return new JwkDiscoveryResponse(jwks);
        }
    }
}
