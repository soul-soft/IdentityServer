namespace IdentityServer.Validation
{
    public class ScopeValidator : IScopeValidator
    {
        private readonly IResourceStore _resources;
        private readonly IdentityServerOptions _options;

        public ScopeValidator(
            IResourceStore resources,
            IdentityServerOptions options)
        {
            _options = options;
            _resources = resources;
        }

        public async Task<Resources> ValidateAsync(IClient client, IEnumerable<string> requestedScopes)
        {
            if (!requestedScopes.Any())
            {
                throw new InvalidScopeException("No scopes found in request");
            }
            if (string.Join("", requestedScopes).Length > _options.InputLengthRestrictions.Scope)
            {
                throw new InvalidScopeException("Scope too long");
            }
            var resources = await _resources.FindByScopeAsync(requestedScopes);
            foreach (var item in requestedScopes)
            {
                if (!client.AllowedScopes.Contains(item))
                {
                    throw new InvalidScopeException(string.Format("Invalid scope:{0}", item));
                }
                if (!resources.Scopes.Contains(item))
                {
                    throw new InvalidScopeException(string.Format("Invalid scope:{0}", item));
                }
            }
            return resources;
        }
    }
}
