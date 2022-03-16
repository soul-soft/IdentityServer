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

        public Task<IEnumerable<string>> GetSecretParserTypesAsync()
        {
            var names = _parsers.Select(s => s.Name);
            return Task.FromResult(names);
        }

        public async Task<ParsedSecret> ParseAsync(HttpContext context)
        {
            var parser = _parsers.Where(a => a.Name == _options.SecretParserType)
                .FirstOrDefault();
            if (parser == null)
            {
                throw new InvalidOperationException($"invalid Secret Parser Type：{_options.SecretParserType}");
            }
            return await parser.ParseAsync(context);
        }
    }
}
