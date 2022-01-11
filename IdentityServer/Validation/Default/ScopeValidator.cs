using IdentityServer.Models;
using IdentityServer.Storage;

namespace IdentityServer.Validation
{
    public class ScopeValidator : IScopeValidator
    {
        public Task<ValidationResult> Validate(IEnumerable<string> allowedScopes, IEnumerable<string> requestedScopes)
        {
            var scopes = requestedScopes;
            foreach (var item in scopes)
            {
                if (!allowedScopes.Contains(item))
                {
                    return ValidationResult.ErrorAsync("Invalid scope: {0}");
                }
            }
           
            return ValidationResult.SuccessAsync();
        }
    }
}
