using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using IdentityServer.Authentication;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IScopeParser _scopeParser;
        private readonly IScopeValidator _scopeValidator;
        private readonly IUserInfoGenerator _generator;
        private readonly IResourceValidator _resourceValidator;

        public UserInfoEndpoint(
            IClientStore clients,
            IResourceStore resources,
            IScopeParser scopeParser,
            IScopeValidator scopeValidator,
            IResourceValidator resourceValidator,
            IUserInfoGenerator generator)
        {
            _clients = clients;
            _resources = resources;
            _generator = generator;
            _scopeParser = scopeParser;
            _scopeValidator = scopeValidator;
            _resourceValidator = resourceValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var authenticateResult = await context.AuthenticateAsync(IdentityServerAuthenticationDefaults.AuthenticationScheme);
            if (authenticateResult == null || !authenticateResult.Succeeded)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidRequest, "authentication failed");
            }
            var subject = authenticateResult.Principal;
            var sub = subject.GetSubjectId();
            if (string.IsNullOrWhiteSpace(sub))
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidToken, "Sub claim is missing");
            }
            var clientId = subject.GetClientId();
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidToken, "ClientId claim is missing");
            }
            var scopes = await _scopeParser.ParseAsync(subject);
            var client = await _clients.GetAsync(clientId);
            if (client == null)
            {
                return BadRequest(OpenIdConnectTokenErrors.InvalidToken, "Invalid client");
            }
            await _scopeValidator.ValidateAsync(client.AllowedScopes,scopes);
            var resources = await _resources.FindByScopeAsync(scopes);
            await _resourceValidator.ValidateAsync(resources, scopes);
            var response = await _generator.ProcessAsync(new UserInfoRequest(subject, client, resources));
            return new UserInfoResult(response);
        }
    }
}
