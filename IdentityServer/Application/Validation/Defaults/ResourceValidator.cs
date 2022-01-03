using IdentityServer.Models;
using IdentityServer.Storage;

namespace IdentityServer.Application
{
    public class ResourceValidator : IResourceValidator
    {
        private readonly IResourceStore _resources;

        public ResourceValidator(IResourceStore resources)
        {
            _resources = resources;
        }

        public async Task<ResourceValidationResult> ValidateAsync(ResourceValidationRequest request)
        {
            if (!request.Scopes.Any())
            {
                return Error("Request scope must be specified");
            }

            foreach (var item in request.Scopes)
            {
                if (!request.Client.AllowedScopes.Contains(item))
                {
                    return Error(string.Format("The client ID {clientId} does not allow scopes {scope}", request.Client.ClientId, item));
                }
            }

            var resources = await _resources.FindResourcesByScopeNameAsync(request.Scopes);

            if (!resources.Any())
            {
                return Error("There are no resources to access");
            }
            return Success(resources);
        }

        private ResourceValidationResult Error(string message)
        {
            var result = new ResourceValidationResult();
            result.Error(message);
            return result;
        }

        private ResourceValidationResult Success(IEnumerable<Resource> resources)
        {
            var result = new ResourceValidationResult(resources);
            result.Success();
            return result;
        }
    }
}
