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

        public async Task<ResourceCollection> ValidateAsync(IEnumerable<string> allowedScopes, IEnumerable<string> requestedScopes)
        {
            if (!requestedScopes.Any())
            {
                throw new InvalidScopeException("No scopes found in request");
            }
            if (string.Join("", requestedScopes).Length > _options.InputLengthRestrictions.Scope)
            {
                throw new InvalidScopeException("Scope too long");
            }
            var resources = await _resources.GetByScopeAsync(requestedScopes);
            foreach (var item in requestedScopes)
            {
                if (!allowedScopes.Contains(item))
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
