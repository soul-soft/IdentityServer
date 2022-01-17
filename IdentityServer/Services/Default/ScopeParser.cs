using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var result = scopes.SelectMany(s=>s.Split(","));
                return Task.FromResult(result);
            }
            return Task.FromResult(scopes);
        }
    }
}
