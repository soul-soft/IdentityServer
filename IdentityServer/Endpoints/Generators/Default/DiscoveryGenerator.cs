using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class DiscoveryGenerator
        : IDiscoveryGenerator
    {
        private readonly IResourceStore _resources;
        private readonly ISigningCredentialsStore _credentials;
        private readonly ClientSecretParserCollection _secretParsers;
        private readonly ExtensionGrantValidatorCollection _extensionGrantValidators;

        public DiscoveryGenerator(
            IResourceStore resources,
            ClientSecretParserCollection secretParsers,
            ISigningCredentialsStore credentials,
            ExtensionGrantValidatorCollection extensionGrantValidators)
        {
            _resources = resources;
            _credentials = credentials;
            _secretParsers = secretParsers;
            _extensionGrantValidators = extensionGrantValidators;
        }

        public async Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer)
        {
            var configuration = new OpenIdConnectConfiguration
            {
                Issuer = issuer,
                JwksUri = issuer + Constants.EndpointRoutePaths.DiscoveryJwks,
                AuthorizationEndpoint = issuer + Constants.EndpointRoutePaths.Authorize,
                TokenEndpoint = issuer + Constants.EndpointRoutePaths.Token,
                UserInfoEndpoint = issuer + Constants.EndpointRoutePaths.UserInfo
            };
            var grantTypes = _extensionGrantValidators.GetCustomGrantTypes();
            foreach (var item in grantTypes)
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
            var authMethods = _secretParsers.GetAuthenticationMethods(); 
            foreach (var item in authMethods)
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
