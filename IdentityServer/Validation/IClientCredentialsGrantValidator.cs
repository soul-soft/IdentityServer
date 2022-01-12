namespace IdentityServer.Validation
{
    public interface IClientCredentialsGrantValidator
    {
        Task<ValidationResult> ValidateAsync(ClientCredentialsGrantValidationContext context);
    }
}
