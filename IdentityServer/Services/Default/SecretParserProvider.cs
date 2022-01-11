using IdentityServer.Configuration;

namespace IdentityServer.Services
{
    public class SecretParserProvider : ISecretParserProvider
    {
        private readonly IdentityServerOptions _options;

        private readonly IEnumerable<ISecretParser> _parsers;

        public SecretParserProvider(
            IdentityServerOptions options,
            IEnumerable<ISecretParser> parsers)
        {
            _options = options;
            _parsers = parsers;
        }

        public IEnumerable<string> GetAuthenticationMethods()
        {
            return _parsers.Select(s => s.AuthenticationMethod);
        }

        public ISecretParser GetParser()
        {
            return _parsers
                .Where(a => a.AuthenticationMethod == _options.TokenEndpointAuthMethod)
                .First();
        }
    }
}
