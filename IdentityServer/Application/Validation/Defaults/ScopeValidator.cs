using IdentityServer.Models;
using IdentityServer.Storage;

namespace IdentityServer.Application
{
    public class ScopeValidator : IScopeValidator
    {
        private readonly IResourceStore _resources;

        public ScopeValidator(IResourceStore resources)
        {
            _resources = resources;
        }

        public async Task<ValidationResult> ValidateAsync(ScopeValidationRequest request)
        {
            var scopes = request.Scopes
                .Split(',')
                .Where(a=>!string.IsNullOrWhiteSpace(a));
            if (!scopes.Any())
            {
                return ValidationResult.Error("Request scope must be specified");
            }

            foreach (var item in scopes)
            {
                if (!request.Client.AllowedScopes.Contains(item))
                {
                    return ValidationResult.Error("The client ID {clientId} does not allow scopes {scope}", request.Client.ClientId, item);
                }
            }

            var resources = await _resources.FindResourcesByScopeNameAsync(scopes);

            if (!resources.Any())
            {
                return ValidationResult.Error("There are no resources to access");
            }

            return ValidationResult.Success();
        }
    }
}
