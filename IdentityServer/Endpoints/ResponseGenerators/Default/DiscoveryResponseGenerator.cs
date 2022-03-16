﻿using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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

        public async Task<DiscoveryResponse> CreateDiscoveryDocumentAsync(string issuer, string baseUrl)
        {
            var configuration = new OpenIdConnectConfiguration
            {
                Issuer = issuer,
                JwksUri = baseUrl + Constants.EndpointRoutePaths.DiscoveryJwks,
                AuthorizationEndpoint = baseUrl + Constants.EndpointRoutePaths.Authorize,
                TokenEndpoint = baseUrl + Constants.EndpointRoutePaths.Token,
                UserInfoEndpoint = baseUrl + Constants.EndpointRoutePaths.UserInfo
            };
            var grantTypes = _extensionGrantValidators.GetGrantTypes();
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
            var secretParserTypes = await _secretParsers.GetSecretParserTypesAsync();
            foreach (var item in secretParserTypes)
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
