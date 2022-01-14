using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public class SecretsListParser : ISecretsListParser
    {
        private readonly IdentityServerOptions _options;

        private readonly IEnumerable<ISecretResolver> _parsers;

        public SecretsListParser(
            IdentityServerOptions options,
            IEnumerable<ISecretResolver> parsers)
        {
            _options = options;
            _parsers = parsers;
        }

        public IEnumerable<string> GetAuthenticationMethods()
        {
            return _parsers.Select(s => s.AuthenticationMethod);
        }

        public async Task<ClientSecret?> ParseAsync(HttpContext context)
        {
            var parser = _parsers.Where(a => a.AuthenticationMethod == _options.TokenEndpointAuthMethod).First();
            return await parser.ParseAsync(context);
        }
    }
}
