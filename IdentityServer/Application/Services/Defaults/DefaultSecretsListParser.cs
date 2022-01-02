using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DefaultSecretsListParser
        : ISecretsListParser
    {
        private readonly ILogger _logger;

        private readonly IEnumerable<ISecretParser> _providers;

        public DefaultSecretsListParser(
            ILogger<DefaultTokenRequestValidator> logger,
            IEnumerable<ISecretParser> providers)
        {
            _logger = logger;
            _providers = providers;
        }

        public IEnumerable<string> GetAuthenticationMethods()
        {
            foreach (var item in _providers)
            {
                yield return item.AuthenticationMethod;
            }
        }

        public async Task<ParsedSecret> TryParseAsync(HttpContext context)
        {
            foreach (var provider in _providers)
            {
                var secret = await provider.ParseAsync(context);
                if (secret != null && secret.Type != IdentityServerConstants.ParsedSecretTypes.NoSecret)
                {
                    return secret;
                }
            }
            var errorDescription = "Secret not found. Please implement and register the 'icretpasser' interface";
            _logger.LogInformation(errorDescription);
            throw new InvalidOperationException(errorDescription);
        }
    }
}
