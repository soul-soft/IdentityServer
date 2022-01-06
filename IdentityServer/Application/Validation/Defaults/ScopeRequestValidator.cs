using IdentityServer.Models;
using IdentityServer.Storage;

namespace IdentityServer.Application
{
    public class ScopeRequestValidator : IScopeRequestValidator
    {
        public Task<ValidationResult> ValidateAsync(IClient client, string[] scopes)
        {
            if (!scopes.Any())
            {
                return ValidationResult.ErrorAsync("Request scope must be specified");
            }

            foreach (var item in scopes)
            {
                if (!client.AllowedScopes.Contains(item))
                {
                    return ValidationResult.ErrorAsync("Invalid scope:{0}", item);
                }
            }

            return ValidationResult.SuccessAsync();
        }
    }
}
