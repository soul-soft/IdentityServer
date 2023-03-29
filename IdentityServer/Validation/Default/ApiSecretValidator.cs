using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    internal class ApiSecretValidator : IApiSecretValidator
    {
        private readonly IResourceStore _resources;
        private readonly ISecretListParser _secretListParser;
        private readonly ISecretListValidator _secretListValidator;

        public ApiSecretValidator(
            IResourceStore resources,
            ISecretListParser secretParsers,
            ISecretListValidator  secretValidators)
        {
            _resources = resources;
            _secretListParser = secretParsers;
            _secretListValidator = secretValidators;
        }
        
        public async Task<ApiResource> ValidateAsync(HttpContext context)
        {
            var parsedSecret = await _secretListParser.ParseAsync(context);
            if (parsedSecret.Type == ClientSecretTypes.NoSecret)
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "Client credentials is missing");
            }
            var apiResources = await _resources.FindApiResourcesByNameAsync(parsedSecret.ClientId);
            if (!apiResources.Any())
            {
                throw new ValidationException(ValidationErrors.InvalidClient, "No API resource with that name found. aborting");
            }
            if (apiResources.Count() > 1)
            {
                throw new ValidationException(ValidationErrors.InvalidClient, "More than one API resource with that name found. aborting");
            }
            var apiResource = apiResources.First();
            await _secretListValidator.ValidateAsync(parsedSecret, apiResource.ApiSecrets);
            return apiResource;
        }
    }
}
