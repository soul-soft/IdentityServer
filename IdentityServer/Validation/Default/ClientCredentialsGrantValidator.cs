namespace IdentityServer.Validation
{
    internal class ClientCredentialsGrantValidator : IClientCredentialsGrantValidator
    {
        public Task<ValidationResult> ValidateAsync(ClientCredentialsGrantValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
