using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Services;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static IdentityServer.Protocols.OpenIdConnectConstants;

namespace IdentityServer.ResponseGenerators
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly ITokenEndpointAuthMethodProvider _secretParsers;
        private readonly ISigningCredentialStore _credentials;

        public DiscoveryResponseGenerator(
            ITokenEndpointAuthMethodProvider secretParsers,
            ISigningCredentialStore credentials)
        {
            _secretParsers = secretParsers;
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
            configuration.GrantTypesSupported.Add(GrantTypes.ClientCredentials);
            configuration.GrantTypesSupported.Add(GrantTypes.Password);
            configuration.GrantTypesSupported.Add(GrantTypes.AuthorizationCode);
            configuration.GrantTypesSupported.Add(GrantTypes.RefreshToken);
            foreach (var item in _secretParsers.GetAllAuthMethods())
            {
                configuration.TokenEndpointAuthMethodsSupported.Add(item);
            }
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
