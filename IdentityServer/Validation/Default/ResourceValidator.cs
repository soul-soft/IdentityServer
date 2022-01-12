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
            if (resources.OfflineAccess)
            {
                if (!resources.IdentityResources.Any(a => a.Name == StandardScopes.OfflineAccess))
                {
                    return ValidationResult.ErrorAsync("Unsupported scope:'{0}'", StandardScopes.OfflineAccess);
                }
            }
            return ValidationResult.SuccessAsync();
        }
    }
}
