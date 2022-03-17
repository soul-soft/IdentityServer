namespace IdentityServer.Validation
{
    internal class ClientCredentialsRequestValidator : IClientCredentialsRequestValidator
    {
        public Task ValidateAsync(ClientCredentialsRequestValidation context)
        {
            var resources = context.Request.Resources;
            if (resources.IdentityResources.Any())
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "Client cannot request OpenID scopes in client credentials flow");
            }
            return Task.CompletedTask;
        }
    }
}
