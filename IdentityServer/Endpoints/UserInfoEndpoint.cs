using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IScopeParser _scopeParser;
        private readonly IUserInfoResponseGenerator _generator;
        private readonly ITokenParser _tokenParser;
        private readonly ITokenValidator _tokenValidator;
        private readonly IScopeValidator _scopeValidator;

        public UserInfoEndpoint(
            IClientStore clients,
            ITokenParser tokenParser,
            IScopeParser scopeParser,
            ITokenValidator tokenValidator,
            IScopeValidator scopeValidator,
            IUserInfoResponseGenerator generator)
        {
            _clients = clients;
            _generator = generator;
            _tokenParser = tokenParser;
            _scopeParser = scopeParser;
            _tokenValidator = tokenValidator;
            _scopeValidator = scopeValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            try
            {
                var token = await _tokenParser.ParserAsync(context);
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidRequestException("Token is miss");
                }
                var claims = await _tokenValidator.ValidateAsync(token);
                var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, "Local"));
                var sub = subject.GetSubjectId();
                if (string.IsNullOrWhiteSpace(sub))
                {
                    throw new InvalidTokenException("Sub claim is missing");
                }
                var clientId = subject.GetClientId();
                if (string.IsNullOrWhiteSpace(clientId))
                {
                    throw new InvalidTokenException("ClientId claim is missing");
                }
                var client = await _clients.FindByClientIdAsync(clientId);
                if (client == null)
                {
                    throw new InvalidClientException("Invalid client");
                }
                var scopes = await _scopeParser.ParseAsync(subject);
                var resources = await _scopeValidator.ValidateAsync(client.AllowedScopes, scopes);
                var response = await _generator.ProcessAsync(new UserInfoGeneratorRequest(client, subject, resources));
                return new UserInfoResult(response);
            }
            catch (ValidationException ex)
            {
                return Unauthorized(ex.Error, ex.ErrorDescription);
            }
        }
    }
}
