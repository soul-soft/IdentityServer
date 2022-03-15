using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly ITokenParser _tokenParser;
        private readonly IdentityServerOptions _options;
        private readonly IResourceStore _resourceStore;
        private readonly ITokenValidator _tokenValidator;
        private readonly IUserInfoResponseGenerator _generator;

        public UserInfoEndpoint(
            IClientStore clients,
            ITokenParser tokenParser,
            IResourceStore resourceStore,
            IdentityServerOptions options,
            ITokenValidator tokenValidator,
            IUserInfoResponseGenerator generator)
        {
            _clients = clients;
            _options = options;
            _generator = generator;
            _tokenParser = tokenParser;
            _resourceStore = resourceStore;
            _tokenValidator = tokenValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var token = await _tokenParser.ParserAsync(context);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Token is miss");
            }
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (tokenValidationResult.IsError)
            {
                return Unauthorized(tokenValidationResult.Error, tokenValidationResult.ErrorDescription);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(tokenValidationResult.Claims, "Local"));
            if (!subject.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                return Unauthorized(OpenIdConnectErrors.InsufficientScope, $"Checking for expected scope {JwtClaimTypes.Subject} failed");
            }
            var clientId = subject.GetClientId();
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Unauthorized(OpenIdConnectErrors.InsufficientScope, "ClientId claim is missing");
            }
            var client = await _clients.FindByClientIdAsync(clientId);
            if (client == null)
            {
                return Unauthorized(OpenIdConnectErrors.InvalidGrant, "Invalid client");
            }
            var scopes = subject.GetScopes(_options.EmitScopesAsCommaDelimitedStringInJwt);
            var resources = await _resourceStore.FindResourcesByScopesAsync(scopes);
            var response = await _generator.ProcessAsync(subject, client, resources);
            return new UserInfoResult(response);
        }
    }
}
