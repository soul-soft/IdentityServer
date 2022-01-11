using IdentityServer.Models;

namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        public Task<ValidationResult> ValidateAsync(Resources resources, IEnumerable<string> requestedScopes)
        {
            foreach (var item in requestedScopes)
            {
                if (!resources.ApiScopes.Any(a => a.Name == item)
                    && !resources.IdentityResources.Any(a => a.Name == item))
                {
                    return ValidationResult.ErrorAsync("Invalid scope:\"{0}\"", item);
                }
            }
            return ValidationResult.SuccessAsync();
        }
    }
}
