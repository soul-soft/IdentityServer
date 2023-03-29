using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class ClientCredentialsRequestValidator : IClientCredentialsRequestValidator
    {
        public Task<ClientCredentialsValidationResult> ValidateAsync(ClientCredentialsValidationRequest request)
        {
            var resources = request.Resources;
            if (resources.IdentityResources.Any())
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Client cannot request OpenID scopes in client credentials flow");
            }
            return Task.FromResult(new ClientCredentialsValidationResult(Array.Empty<Claim>()));
        }
    }
}
