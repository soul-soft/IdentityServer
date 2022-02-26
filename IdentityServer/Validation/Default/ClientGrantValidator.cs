namespace IdentityServer.Validation
{
    internal class ClientGrantValidator: IClientGrantValidator
    {
        public Task ValidateAsync(ClientGrantValidationRequest context)
        {
            var resources = context.Request.Resources;
            if (resources.IdentityResources.Any())
            {
                throw new InvalidGrantException("Client cannot request OpenID scopes in client credentials flow");
            }
            return Task.CompletedTask;
        }
    }
}
