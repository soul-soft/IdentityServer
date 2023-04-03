using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly IResourceStore _resources;
        private readonly EndpointDescriptors _endpoints;
        private readonly IdentityServerOptions _options;
        private readonly ISecretListParser _secretParsers;
        private readonly ISigningCredentialsService _credentials;
        private readonly IExtensionGrantListValidator _extensionGrantValidators;

        public DiscoveryResponseGenerator(
            IResourceStore resources,
            EndpointDescriptors endpoints,
            IdentityServerOptions options,
            ISecretListParser secretParsers,
            ISigningCredentialsService credentials,
            IExtensionGrantListValidator extensionGrantValidators)
        {
            _resources = resources;
            _options = options;
            _endpoints = endpoints;
            _credentials = credentials;
            _secretParsers = secretParsers;
            _extensionGrantValidators = extensionGrantValidators;
        }

        public async Task<DiscoveryGeneratorResponse> GetDiscoveryDocumentAsync(string issuer, string baseUrl)
        {
            var configuration = new OpenIdConnectConfiguration
            {
                Issuer = issuer,
                JwksUri = baseUrl + _endpoints.GetEndpoint(OpenIdConnectConstants.EndpointNames.DiscoveryJwks),
                AuthorizationEndpoint = baseUrl + _endpoints.GetEndpoint(OpenIdConnectConstants.EndpointNames.Authorize),
                TokenEndpoint = baseUrl + _endpoints.GetEndpoint(OpenIdConnectConstants.EndpointNames.Token),
                UserInfoEndpoint = baseUrl + _endpoints.GetEndpoint(OpenIdConnectConstants.EndpointNames.UserInfo),
                IntrospectionEndpoint = baseUrl + _endpoints.GetEndpoint(OpenIdConnectConstants.EndpointNames.Introspection),
            };
            var revocationEndpoint = baseUrl + _endpoints.GetEndpoint(OpenIdConnectConstants.EndpointNames.Revocation);
            configuration.AdditionalData.Add("revocation_endpoint", revocationEndpoint);
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
