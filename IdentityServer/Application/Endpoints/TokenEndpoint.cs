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
        private readonly IResourceStore _resources;
        private readonly ITokenService _tokenService;
        private readonly IScopeRequestValidator _scopeRequestValidator;
        private readonly IClientSecretValidator _clientSecretValidator;
        private readonly ITokenRequestValidator _tokenRequestValidator;

        public TokenEndpoint(
            IClientStore clients,
            IServerUrls serverUrls,
            ITokenService tokenService,
            ILogger<TokenEndpoint> logger,
            IResourceStore resources,
            ICredentialParser credentialParser,
            IScopeRequestValidator scopeRequestValidator,
            IClientSecretValidator clientSecretValidator,
            ITokenRequestValidator tokenRequestValidator)
        {
            _logger = logger;
            _clients = clients;
            _resources = resources;
            _serverUrls = serverUrls;
            _tokenService = tokenService;
            _credentialParser = credentialParser;
            _scopeRequestValidator = scopeRequestValidator;
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
            validationResult = await _tokenRequestValidator.ValidateAsync(client, context);
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidRequest);
            }
            var parameters = await context.Request.ReadFormAsNameValueCollectionAsync();
            var scope = parameters.Get(OidcConstants.TokenRequest.Scope);
            if (string.IsNullOrWhiteSpace(scope))
            {
                scope = string.Join(",", client.AllowedScopes);
            }
            var scopes = scope.Split(',');
            var resources = await _resources.FindResourcesByScopesAsync(scopes);
            validationResult = await _scopeRequestValidator.ValidateAsync(client, scopes);
            if (validationResult.IsError)
            {
                _logger.LogError(validationResult.Description);
                return Error(OidcConstants.TokenErrors.InvalidRequest);
            }
            var issuerUrl = _serverUrls.GetIdentityServerIssuerUri();
            var tokenRequest = new TokenRequest(client, issuerUrl, OidcConstants.TokenTypes.AccessToken)
            {
                SessionId = Guid.NewGuid().ToString("N"),
                Scopes = scopes,
                UserClaims = resources.SelectMany(s => s.UserClaims).ToList()
            };
            var token = await _tokenService.CreateIdentityTokenAsync(tokenRequest);
            var accessToken = await _tokenService.CreateSecurityTokenAsync(token);
            return new TokenResult(new TokenResponse()
            {
                AccessToken = accessToken,
                ExpiresIn = tokenRequest.Lifetime,
                Scope = scope
            });
        }

        private TokenErrorResult Error(string error, string? errorDescription = null)
        {
            return new TokenErrorResult(new TokenErrorResponse(error, errorDescription));
        }
    }
}
