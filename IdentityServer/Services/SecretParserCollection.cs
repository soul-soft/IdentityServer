using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public class SecretParserCollection
    {
        private readonly IdentityServerOptions _options;

        private readonly IEnumerable<ISecretParser> _parsers;

        public SecretParserCollection(
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

        public async Task<ParsedCredentials> ParseAsync(HttpContext context)
        {
            var parser = _parsers
                .Where(a => a.AuthenticationMethod == _options.TokenEndpointAuthMethod).
                FirstOrDefault();
            if (parser == null)
            {
                throw new InvalidOperationException($"invalid authentication method：{_options.TokenEndpointAuthMethod}");
            }
            return await parser.ParseAsync(context);
        }
    }
}
