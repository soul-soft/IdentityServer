using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class TokenResponseGenerator
        : ITokenResponseGenerator
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly IClientSecretValidator _clientValidator;
        private readonly ITokenRequestValidator _requestValidator;

        public TokenResponseGenerator(
            ILogger<TokenResponseGenerator> logger,
            ITokenService tokenService,
            IClientSecretValidator clientValidator,
            ITokenRequestValidator requestValidator)
        {
            _logger = logger;
            _tokenService = tokenService;
            _clientValidator = clientValidator;
            _requestValidator = requestValidator;
        }

        public async Task<TokenResponse> ProcessAsync(HttpContext context)
        {
            //验证客户端
            var clientValidationResult = await _clientValidator.ValidateAsync(context);

            if (clientValidationResult.IsError)
            {
                _logger.LogError(clientValidationResult.Description);
            }

            var parameters = await context.Request.ReadFormAsNameValueCollectionAsync();
            //验证请求
            var tokenValidationResult = await _requestValidator.ValidateRequestAsync(parameters, clientValidationResult);
            //创建token
            var token = await _tokenService.CreateAccessTokenAsync(new TokenCreationRequest
            {
                AccessTokenLifetime = tokenValidationResult.Client.AccessTokenLifetime,
                AccessTokenType = tokenValidationResult.Client.AccessTokenType,
            });
            //通过密钥加密
            var accessToken = await _tokenService.CreateSecurityTokenAsync(token);
            var response = new TokenResponse()
            {
                AccessToken = accessToken,
                ExpiresIn = token.Lifetime,
                Scope = tokenValidationResult.Scopes
            };
            return response;
        }
    }
}
