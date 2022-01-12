namespace IdentityServer.Validation
{
    internal class ClientCredentialsGrantValidator
        : IClientCredentialsGrantValidator
    {
        public Task<ValidationResult> ValidateAsync(ClientCredentialsGrantValidationContext context)
        {
            var resources = context.Resources;
            if (resources.IdentityResources.Any())
            {
                return ValidationResult.ErrorAsync("Client cannot request OpenID scopes in client credentials flow");
            }
            return ValidationResult.SuccessAsync();
        }
    }
}
