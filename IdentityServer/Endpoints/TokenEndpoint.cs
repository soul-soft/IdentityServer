using IdentityServer.Configuration;
using IdentityServer.Hosting;
using IdentityServer.Models;
using IdentityServer.ResponseGenerators;
using IdentityServer.Services;
using IdentityServer.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using IdentityServer.Protocols;
using Microsoft.Extensions.Logging;
using IdentityServer.Extensions;

namespace IdentityServer.Endpoints
{
    public class TokenEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IdentityServerOptions _options;
        private readonly ILogger<TokenEndpoint> _logger;
        private readonly ITokenResponseGenerator _generator;
        private readonly ITokenEndpointAuthMethodProvider _authMethodProvider;

        public TokenEndpoint(
            IClientStore clients,
            IResourceStore resources,
            IdentityServerOptions options,
            ILogger<TokenEndpoint> logger,
            ITokenResponseGenerator generator,
            ITokenEndpointAuthMethodProvider authMethodProvider)
        {
            _logger = logger;
            _clients = clients;
            _options = options;
            _resources = resources;
            _generator = generator;
            _authMethodProvider = authMethodProvider;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!_options.Endpoints.EnableTokenEndpoint)
            {
                return MethodNotAllowed();
            }
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasApplicationFormContentType())
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidRequest);
            }
            var authMethod = await _authMethodProvider.GetDefaultAuthMethodAsync();
            var clientSecret = await authMethod.ParseAsync(context);
            if (clientSecret == null)
            {
                _logger.LogError("The client with secret cannot be found in the '{0}' authorization method", authMethod.AuthMethod);
                return BadRequest(OpenIdConnectTokenErrors.InvalidRequest);
            }
            var client = await _clients.FindClientByIdAsync(clientSecret.Id);
            if (client == null)
            {
                _logger.LogError("No client with id '{clientId}' found. aborting", clientSecret.Id);
                return BadRequest(OpenIdConnectTokenErrors.InvalidClient);
            }
            var form = await context.Request.ReadFormAsync();
            var scope = form[OpenIdConnectParameterNames.Scope].FirstOrDefault();
            if (scope != null && scope.Length > _options.InputLengthRestrictions.Scope)
            {
                _logger.LogError("Scope parameter exceeds max allowed length");
                return BadRequest(OpenIdConnectTokenErrors.InvalidScope);
            }
            if (string.IsNullOrEmpty(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = scope.Split(",").Where(a => !string.IsNullOrEmpty(a));
            var resources = await _resources.FindResourcesByScopeAsync(scopes);
            var grantType = form[OpenIdConnectParameterNames.GrantType].FirstOrDefault();
            if (string.IsNullOrEmpty(grantType))
            {
                return BadRequest(OpenIdConnectTokenErrors.UnsupportedGrantType);
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                _logger.LogError("Grant type is too long");
                return BadRequest(OpenIdConnectTokenErrors.UnsupportedGrantType);
            }
            var response = await _generator.ProcessAsync(new TokenRequest(client, resources));
            return TokenResult(response);
        }

    }
}
