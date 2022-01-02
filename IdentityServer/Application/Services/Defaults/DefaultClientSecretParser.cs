using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class DefaultClientSecretParser
        : IClientSecretParser
    {
        private readonly ILogger _logger;

        private readonly IEnumerable<IClientSecretScheme> _providers;

        public DefaultClientSecretParser(
            ILogger<DefaultClientSecretValidator> logger,
            IEnumerable<IClientSecretScheme> providers)
        {
            _logger = logger;
            _providers = providers;
        }

        public IEnumerable<string> GetAvailableAuthenticationMethods()
        {
            foreach (var item in _providers)
            {
                yield return item.AuthenticationMethod;
            }
        }

        public async Task<ParsedSecret?> ParseAsync(HttpContext context)
        {
            foreach (var provider in _providers)
            {
                var secret = await provider.ParseAsync(context);
                if (secret != null)
                {
                    _logger.LogDebug("Parser found secret: {type}", provider.GetType().Name);
                    return secret;
                }
            }
            return null;
        }
    }
}
