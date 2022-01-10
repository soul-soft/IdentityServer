using IdentityServer.Configuration;

namespace IdentityServer.Services
{
    public class TokenEndpointAuthMethodProvider 
        : ITokenEndpointAuthMethodProvider
    {
        private readonly IdentityServerOptions _options;
     
        private readonly IEnumerable<ITokenEndpointAuthMethod> _authMethods;

        public TokenEndpointAuthMethodProvider(
            IdentityServerOptions options,
            IEnumerable<ITokenEndpointAuthMethod> authMethods)
        {
            _options = options;
            _authMethods = authMethods;
        }

        public IEnumerable<string> GetAllAuthMethods()
        {
            return _authMethods.Select(s => s.AuthMethod);
        }

        public Task<ITokenEndpointAuthMethod> GetDefaultAuthMethodAsync()
        {
            ITokenEndpointAuthMethod? authMethod = null;
            foreach (var item in _options.TokenEndpointAuthMethods)
            {
                authMethod = _authMethods.Where(a => a.AuthMethod == item).FirstOrDefault();
                if (authMethod != null)
                {
                    break;
                }
            }
            if (authMethod == null)
            {
                authMethod = _authMethods.First();
            }
            return Task.FromResult(authMethod);
        }

        public Task<ITokenEndpointAuthMethod?> GetAuthMethodAsync(string name)
        {
            var authMethod = _authMethods
                .Where(a => a.AuthMethod == name).FirstOrDefault();
            return Task.FromResult(authMethod);
        }
    }
}
