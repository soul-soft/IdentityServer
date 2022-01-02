using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class SecretsListParser
        : ISecretsListParser
    {
        private readonly IEnumerable<ISecretParser> _providers;

        public SecretsListParser(IEnumerable<ISecretParser> providers)
        {
            _providers = providers;
        }

        public IEnumerable<string> GetAuthenticationMethods()
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
                if (secret != null && secret.Type != IdentityServerConstants.ParsedSecretTypes.NoSecret)
                {
                    return secret;
                }
            }
            return null;
        }
    }
}
