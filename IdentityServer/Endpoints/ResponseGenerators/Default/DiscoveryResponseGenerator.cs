using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class DiscoveryResponseGenerator
        : IDiscoveryResponseGenerator
    {
        private readonly IResourceStore _resources;
        private readonly IdentityServerOptions _options;
        private readonly ISecretListParser _secretParsers;
        private readonly ISigningCredentialsStore _credentials;
        private readonly IExtensionGrantListValidator _extensionGrantValidators;

        public DiscoveryResponseGenerator(
            IResourceStore resources,
            IdentityServerOptions options,
            ISecretListParser secretParsers,
            ISigningCredentialsStore credentials,
            IExtensionGrantListValidator extensionGrantValidators)
        {
            _resources = resources;
            _options = options;
            _credentials = credentials;
            _secretParsers = secretParsers;
            _extensionGrantValidators = extensionGrantValidators;
        }

        public async Task<DiscoveryGeneratorResponse> GetDiscoveryDocumentAsync(string issuer, string baseUrl)
        {
            var configuration = new OpenIdConnectConfiguration
            {
                Issuer = issuer,
                JwksUri = baseUrl + _options.Endpoints.GetEndpointFullPath(Constants.EndpointRutePaths.DiscoveryJwks),
                AuthorizationEndpoint = baseUrl + _options.Endpoints.GetEndpointFullPath(Constants.EndpointRutePaths.Authorize),
                TokenEndpoint = baseUrl + _options.Endpoints.GetEndpointFullPath(Constants.EndpointRutePaths.Token),
                UserInfoEndpoint = baseUrl + _options.Endpoints.GetEndpointFullPath(Constants.EndpointRutePaths.UserInfo),
                IntrospectionEndpoint = baseUrl + _options.Endpoints.GetEndpointFullPath(Constants.EndpointRutePaths.Introspection),
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
