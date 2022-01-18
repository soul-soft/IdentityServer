using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ScopeParser : IScopeParser
    {
        private readonly IdentityServerOptions _options;

        public ScopeParser(IdentityServerOptions options)
        {
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
                return Task.FromResult<IEnumerable<string>>(scopes);
            }
        }
    }
}
