using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using IdentityServer.Hosting;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IScopeParser _scopeParser;
        private readonly IUserInfoGenerator _generator;
        private readonly ITokenParser _tokenParser;
        private readonly ITokenValidator _tokenValidator;
        private readonly IScopeValidator _scopeValidator;

        public UserInfoEndpoint(
            IClientStore clients,
            IResourceStore resources,
            ITokenParser tokenParser,
            IScopeParser scopeParser,
            ITokenValidator tokenValidator,
            IScopeValidator scopeValidator,
            IUserInfoGenerator generator)
        {
            _clients = clients;
            _resources = resources;
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
                var subject = await _tokenValidator.ValidateAsync(token);
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
                var scopes = await _scopeParser.ParseAsync(subject);
                var client = await _clients.GetAsync(clientId);
                if (client == null)
                {
                    throw new InvalidClientException("Invalid client");
                }
                var resources = await _scopeValidator.ValidateAsync(client, scopes);
                var response = await _generator.ProcessAsync(new UserInfoRequest(subject, client, resources));
                return new UserInfoResult(response);
            }
            catch (InvalidException ex)
            {
                return Unauthorized(ex.Error, ex.ErrorDescription);
            }
        }
    }
}
