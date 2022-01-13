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
                return GrantValidationResult.ErrorAsync("Client cannot request OpenID scopes in client credentials flow");
            }
            
            return GrantValidationResult.SuccessAsync();
        }
    }
}
