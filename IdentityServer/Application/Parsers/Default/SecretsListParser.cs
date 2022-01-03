using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class SecretsListParser: ISecretListParser
    {
        private readonly IEnumerable<ISecretParser> _parsers;

        public SecretsListParser(IEnumerable<ISecretParser> parsers)
        {
            _parsers = parsers;
        }

        public IEnumerable<string> GetAuthenticationMethods()
        {
            foreach (var item in _parsers)
            {
                yield return item.AuthenticationMethod;
            }
        }

        public async Task<ParsedSecret?> ParseAsync(HttpContext context)
        {
            foreach (var provider in _parsers)
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
