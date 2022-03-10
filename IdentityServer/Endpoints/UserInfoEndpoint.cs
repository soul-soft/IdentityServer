using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IScopeParser _scopeParser;
        private readonly ITokenParser _tokenParser;
        private readonly IResourceStore _resourceStore;
        private readonly ITokenValidator _tokenValidator;
        private readonly IUserInfoResponseGenerator _generator;

        public UserInfoEndpoint(
            IClientStore clients,
            ITokenParser tokenParser,
            IScopeParser scopeParser,
            IResourceStore resourceStore,
            ITokenValidator tokenValidator,
            IUserInfoResponseGenerator generator)
        {
            _clients = clients;
            _generator = generator;
            _tokenParser = tokenParser;
            _scopeParser = scopeParser;
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
            var claims = await _tokenValidator.ValidateAccessTokenAsync(token);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, "Local"));
            var sub = subject.GetSubjectId();
            if (string.IsNullOrWhiteSpace(sub))
            {
                return Unauthorized(OpenIdConnectErrors.InsufficientScope, $"Checking for expected scope {JwtClaimTypes.Subject} failed");
            }
            var clientId = subject.GetClientId();
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Unauthorized(OpenIdConnectErrors.InvalidGrant, "ClientId claim is missing");
            }
            var client = await _clients.FindByClientIdAsync(clientId);
            if (client == null)
            {
                return Unauthorized(OpenIdConnectErrors.InvalidGrant, "Invalid client");
            }
            var scopes = await _scopeParser.ParseAsync(subject);
            var resources = await _resourceStore.FindResourceByScopesAsync(scopes);
            var response = await _generator.ProcessAsync(new UserInfoGeneratorRequest(client, subject, resources));
            return new UserInfoResult(response);
        }
    }
}
