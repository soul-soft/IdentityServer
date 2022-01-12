namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        public Task<ValidationResult> ValidateAsync(Resources resources, IEnumerable<string> requestedScopes)
        {
            foreach (var scope in requestedScopes)
            {
                if (!resources.Scopes.Contains(scope))
                {
                    return ValidationResult.ErrorAsync("Invalid scope:'{0}'", scope);
                }
            }
            return ValidationResult.SuccessAsync();
        }
    }
}
