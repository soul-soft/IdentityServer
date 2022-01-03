namespace IdentityServer.Application
{
    public interface IClientCredentialsGrantValidator
    {
        Task<ValidationResult> ValidateAsync(ClientCredentialsGrantValidationRequest context);
    }
}
