namespace IdentityServer.Validation
{
    internal class ClientCredentialsGrantValidator
        : IClientCredentialsGrantValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ClientCredentialsGrantValidationContext context)
        {
            var resources = context.Request.Resources;
            if (resources.IdentityResources.Any())
            {
                throw new InvalidGrantException("Client cannot request OpenID scopes in client credentials flow");
            }
            var result = new GrantValidationResult();
            return Task.FromResult(result);
        }
    }
}
