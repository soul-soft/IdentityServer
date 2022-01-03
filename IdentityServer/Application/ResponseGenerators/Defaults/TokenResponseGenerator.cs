using IdentityServer.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class TokenResponseGenerator
        : ITokenResponseGenerator
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly ISecretListParser _secretsListParser;
        private readonly ISecretValidator _secretsListValidator;
        private readonly IClientValidator _clientValidator;
        private readonly IClientStore _clientStore;
        private readonly ITokenRequestValidator _requestValidator;

        public TokenResponseGenerator(
            ILogger<TokenResponseGenerator> logger,
            ITokenService tokenService,
            ISecretListParser secretsListParser,
            ISecretValidator secretsListValidator,
            IClientValidator clientValidator,
            IClientStore clientStore,
            ITokenRequestValidator requestValidator)
        {
            _logger = logger;
            _tokenService = tokenService;
            _secretsListParser = secretsListParser;
            _secretsListValidator = secretsListValidator;
            _clientValidator = clientValidator;
            _clientStore = clientStore;
            _requestValidator = requestValidator;
        }

        public async Task<TokenResponse> ProcessAsync(HttpContext context)
        {
            
            return new TokenResponse();
        }
    }
}
