using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly IIdentityServerUrl _urls;
        private readonly IResourceStore _resources;
        private readonly ISecretListParser _secretParsers;
        private readonly ISigningCredentialsService _credentials;
        private readonly IExtensionGrantListValidator _extensionGrantValidators;

        public DiscoveryResponseGenerator(
            IIdentityServerUrl urls,
            IResourceStore resources,
            ISecretListParser secretParsers,
            ISigningCredentialsService credentials,
            IExtensionGrantListValidator extensionGrantValidators)
        {
            _urls = urls;
            _resources = resources;
            _credentials = credentials;
            _secretParsers = secretParsers;
            _extensionGrantValidators = extensionGrantValidators;
        }

        public async Task<DiscoveryGeneratorResponse> GetDiscoveryDocumentAsync()
        {
            var configuration = new OpenIdConnectConfiguration
            {
                Issuer = _urls.GetServerIssuer(),
                JwksUri = _urls.GetEndpointUri(IdentityServerEndpointNames.DiscoveryJwks),
                TokenEndpoint = _urls.GetEndpointUri(IdentityServerEndpointNames.Token),
                UserInfoEndpoint = _urls.GetEndpointUri(IdentityServerEndpointNames.UserInfo),
                EndSessionEndpoint = _urls.GetEndpointUri(IdentityServerEndpointNames.EndSession),
                AuthorizationEndpoint = _urls.GetEndpointUri(IdentityServerEndpointNames.Authorize),
                IntrospectionEndpoint = _urls.GetEndpointUri(IdentityServerEndpointNames.Introspection),
            };
            configuration.AdditionalData.Add("revocation_endpoint", _urls.GetEndpointUri(IdentityServerEndpointNames.Revocation));
            var supportedExtensionsGrantTypes = _extensionGrantValidators.GetSupportedGrantTypes();
            foreach (var item in supportedExtensionsGrantTypes)
            {
                configuration.GrantTypesSupported.Add(item);
            }
            configuration.GrantTypesSupported.Add(GrantTypes.ClientCredentials);
            configuration.GrantTypesSupported.Add(GrantTypes.Password);
            configuration.GrantTypesSupported.Add(GrantTypes.RefreshToken);
            configuration.GrantTypesSupported.Add(GrantTypes.AuthorizationCode);
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
