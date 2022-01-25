using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ScopeParser : IScopeParser
    {
        private readonly IClient _client;
        private readonly IdentityServerOptions _options;

        public ScopeParser(
            IClient client,
            IdentityServerOptions options)
        {
            _client = client;
            _options = options;
        }

        public Task<IEnumerable<string>> ParseAsync(IEnumerable<string> scopes)
        {
            if (_options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                var result = scopes.SelectMany(s => s.Split(","));
                return Task.FromResult(result);
            }
            return Task.FromResult(scopes);
        }

        public Task<IEnumerable<string>> ParseAsync(ClaimsPrincipal subject)
        {
            if (_options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                var scope = subject.FindFirstValue(JwtClaimTypes.Scope);
                var scopes = scope.Split(",");
                return Task.FromResult<IEnumerable<string>>(scopes);
            }
            else
            {
                var scopes = subject.FindAll(JwtClaimTypes.Scope).Select(s => s.Value);
                return Task.FromResult(scopes);
            }
        }

        public Task<IEnumerable<string>> ParseAsync(string? scope)
        {
            if (string.IsNullOrWhiteSpace(scope))
            {
                return Task.FromResult<IEnumerable<string>>(_client.AllowedScopes);
            }
            var scopes = scope.Split(',');
            return Task.FromResult<IEnumerable<string>>(scopes);
        }
    }
}
