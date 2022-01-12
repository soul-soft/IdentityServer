namespace IdentityServer.Validation
{
    public class ScopeValidator : IScopeValidator
    {
        private readonly IdentityServerOptions _options;
       
        public ScopeValidator(IdentityServerOptions options)
        {
            _options=options;
        }
      
        public Task<ValidationResult> Validate(IEnumerable<string> allowedScopes, IEnumerable<string> requestedScopes)
        {
            if (!allowedScopes.Any())
            {
                return ValidationResult.ErrorAsync("No allowed scopes configured for client");
            }
            if (!requestedScopes.Any())
            {
                return ValidationResult.ErrorAsync("No scopes found in request");
            }
            var scope = string.Join("", requestedScopes);
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                return ValidationResult.ErrorAsync("Scope parameter exceeds max allowed length");
            }
            foreach (var item in requestedScopes)
            {
                if (!allowedScopes.Contains(item))
                {
                    return ValidationResult.ErrorAsync("Unsupported client scope: '{0}'",item);
                }
            }
            return ValidationResult.SuccessAsync();
        }
    }
}
