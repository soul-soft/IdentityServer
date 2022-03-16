using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public class SecretListParser: ISecretListParser
    {
        private readonly IdentityServerOptions _options;

        private readonly IEnumerable<ISecretParser> _parsers;

        public SecretListParser(
            IdentityServerOptions options,
            IEnumerable<ISecretParser> parsers)
        {
            _options = options;
            _parsers = parsers;
        }

        public IEnumerable<string> GetSecretParserTypes()
        {
            return _parsers.Select(s => s.ParserType);
        }

        public async Task<ParsedSecret> ParseAsync(HttpContext context)
        {
            var parser = _parsers.Where(a => a.ParserType == _options.SecretParserType)
                .FirstOrDefault();
            if (parser == null)
            {
                throw new InvalidOperationException($"invalid authentication method：{_options.SecretParserType}");
            }
            return await parser.ParseAsync(context);
        }
    }
}
