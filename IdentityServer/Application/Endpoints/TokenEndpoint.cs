using IdentityModel;
using IdentityServer.Hosting;
using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class TokenEndpoint : IEndpointHandler
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IServerUrls _urls;
        private readonly IClientStore _clients;
        private readonly ISecretListParser _secretListParser;
        private readonly ISecretValidator _secretListValidator;
        private readonly IClientValidator _clientValidator;
        private readonly ITokenRequestValidator _requestValidator;

        public TokenEndpoint(
            IClientStore clients,
            ISecretListParser secretListParser,
            ISecretValidator secretListValidator,
            IClientValidator clientValidator,
            ITokenRequestValidator requestValidator,
            ILogger<TokenEndpoint> logger,
            ITokenService tokenService,
            IServerUrls serverUrls)
        {
            _logger = logger;
            _clients = clients;
            _secretListParser = secretListParser;
            _secretListValidator = secretListValidator;
            _clientValidator = clientValidator;
            _requestValidator = requestValidator;
            _tokenService = tokenService;
            _urls = serverUrls;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            //解析凭据
            var parsedCredential = await _secretListParser.ParseAsync(context);
            if (parsedCredential == null)
            {
                return Error(OidcConstants.TokenErrors.InvalidClient);
            }
            var client = await _clients.FindClientByIdAsync(parsedCredential.Id);
            if (client == null)
            {
                return Error(OidcConstants.TokenErrors.InvalidClient);
            }
            //验证客户端
            var validationResult = await _clientValidator.ValidateAsync(new ClientValidationRequest(client));
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidClient);
            }
            //验证凭据
            validationResult = await _secretListValidator.ValidateAsync(new SecretValidationRequest(client.ClientSecrets, parsedCredential));
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.UnauthorizedClient);
            }
            //验证请求
            var parameters = await context.Request.ReadFormAsNameValueCollectionAsync();
            validationResult = await _requestValidator.ValidateRequestAsync(new TokenRequestValidationRequest(client, parameters));
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidRequest);
            }
            return new TokenResult(null);
        }

        private TokenErrorResult Error(string error, string? errorDescription = null)
        {
            return new TokenErrorResult(new TokenErrorResponse(error, errorDescription));
        }
    }
}
