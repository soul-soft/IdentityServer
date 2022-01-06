using IdentityModel;
using IdentityServer.Hosting;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class TokenEndpoint : IEndpointHandler
    {
        private readonly ILogger _logger;
        private readonly IServerUrls _serverUrls;
        private readonly ICredentialParser _credentialParser;
        private readonly IClientStore _clients;
        private readonly IClientSecretValidator _clientSecretValidator;
        private readonly ITokenRequestValidator _tokenRequestValidator;
        private readonly ITokenService _tokenService;

        public TokenEndpoint(
            IClientStore clients,
            IServerUrls serverUrls,
            ITokenService tokenService,
            ILogger<TokenEndpoint> logger,
            ICredentialParser credentialParser,
            IClientSecretValidator clientSecretValidator,
            ITokenRequestValidator tokenRequestValidator)
        {
            _logger = logger;
            _clients = clients;
            _serverUrls = serverUrls;
            _tokenService = tokenService;
            _credentialParser = credentialParser;
            _clientSecretValidator = clientSecretValidator;
            _tokenRequestValidator = tokenRequestValidator;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            //获取凭证
            var credential = await _credentialParser.ParseAsync(context);
            //获取客户端
            var client = await _clients.FindClientByIdAsync(credential.Id);
            if (client == null)
            {
                return Error(OidcConstants.TokenErrors.InvalidClient);
            }
            //验证客户端凭证
            var validationResult = await _clientSecretValidator.ValidateAsync(client, credential);
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.UnauthorizedClient);
            }
            //验证请求
            validationResult = await _tokenRequestValidator.ValidateRequestAsync(context);
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidRequest);
            }
            var issuerUrl = _serverUrls.GetIdentityServerIssuerUri();
            var accessToken = await _tokenService.CreateAccessTokenAsync(new TokenCreationRequest(issuerUrl, client) 
            {
            });
            return new TokenResult(new TokenResponse());
        }

        private TokenErrorResult Error(string error, string? errorDescription = null)
        {
            return new TokenErrorResult(new TokenErrorResponse(error, errorDescription));
        }
    }
}
