namespace IdentityServer.Validation
{
    public class ScopeValidator : IScopeValidator
    {
        private readonly IdentityServerOptions _options;

        public ScopeValidator(IdentityServerOptions options)
        {
            _options = options;
        }

        public Task ValidateAsync(IEnumerable<string> allowedScopes, IEnumerable<string> requestedScopes)
        {
            if (!allowedScopes.Any())
            {
                throw new InvalidScopeException("No allowed scopes configured for client");
            }
            if (!requestedScopes.Any())
            {
                throw new InvalidScopeException("No scopes found in request");
            }
            var scope = string.Join("", requestedScopes);
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                throw new InvalidScopeException("Scope too long");
            }
            foreach (var item in requestedScopes)
            {
                if (!allowedScopes.Contains(item))
                {
                    throw new InvalidScopeException(string.Format("Unsupported client scope: '{0}'", item));
                }
            }
            return Task.CompletedTask;
        }
    }
}
