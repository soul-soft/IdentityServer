using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly IResourceStore _resources;
        private readonly ISigningCredentialStore _credentials;
        private readonly ISecretListParser _secretParsers;
        private readonly IExtensionGrantListValidator _extensionGrantValidators;

        public DiscoveryResponseGenerator(
            IResourceStore resources,
            ISecretListParser secretParsers,
            ISigningCredentialStore credentials,
            IExtensionGrantListValidator extensionGrantValidators)
        {
            _resources = resources;
            _credentials = credentials;
            _secretParsers = secretParsers;
            _extensionGrantValidators = extensionGrantValidators;
        }

        public async Task<DiscoveryGeneratorResponse> CreateDiscoveryDocumentAsync(string issuer, string baseUrl)
        {
            var configuration = new OpenIdConnectConfiguration
            {
                Issuer = issuer,
                JwksUri = baseUrl + Constants.EndpointRoutePaths.DiscoveryJwks,
                AuthorizationEndpoint = baseUrl + Constants.EndpointRoutePaths.Authorize,
                TokenEndpoint = baseUrl + Constants.EndpointRoutePaths.Token,
                UserInfoEndpoint = baseUrl + Constants.EndpointRoutePaths.UserInfo
            };
            var supportedExtensionsGrantTypes = _extensionGrantValidators.GetSupportedGrantTypes();
            foreach (var item in supportedExtensionsGrantTypes)
            {
                configuration.GrantTypesSupported.Add(item);
            }
            configuration.GrantTypesSupported.Add(GrantTypes.ClientCredentials);
            configuration.GrantTypesSupported.Add(GrantTypes.Password);
            configuration.GrantTypesSupported.Add(GrantTypes.RefreshToken);
            var scopes = await _resources.GetShowInDiscoveryDocumentScopesAsync();
            foreach (var item in scopes)
            {
                configuration.ScopesSupported.Add(item);
            }
            var supportedAuthenticationMethods = await _secretParsers.GetSupportedAuthenticationMethodsAsync();
            foreach (var item in supportedAuthenticationMethods)
            {
                configuration.TokenEndpointAuthMethodsSupported.Add(item);
            }
            var response = new DiscoveryGeneratorResponse(configuration);
            return response;
        }

        public async Task<JwkDiscoveryGeneratorResponse> CreateJwkDiscoveryDocumentAsync()
        {
            var jwks = await _credentials.GetJsonWebKeysAsync();
            return new JwkDiscoveryGeneratorResponse(jwks);
        }
    }
}
