namespace IdentityServer.Validation
{
    public class ResourceValidator : IResourceValidator
    {
        private readonly IResourceStore _resources;

        public ResourceValidator(IResourceStore resources)
        {
            _resources = resources;
        }

        public async Task<Resources> ValidateAsync(Client client, IEnumerable<string> scopes)
        {
            if (!scopes.Any())
            {
                if (!client.AllowedScopes.Any())
                {
                    throw new ValidationException(OpenIdConnectValidationErrors.InvalidScope, "No allowed scopes configured for client");
                }
                scopes = client.AllowedScopes;
            }
            else
            {
                foreach (var item in scopes)
                {
                    if (!client.AllowedScopes.Contains(item))
                    {
                        throw new ValidationException(OpenIdConnectValidationErrors.InvalidScope, $"Scope '{item}' not allowed");
                    }
                }
            }
            var resources = await _resources.FindResourcesByScopesAsync(scopes);
            foreach (var scope in scopes)
            {
                if (!resources.Scopes.Contains(scope))
                {
                    throw new ValidationException($"Invalid scope: {scope}");
                }
            }
            return resources;
        }
    }
}
