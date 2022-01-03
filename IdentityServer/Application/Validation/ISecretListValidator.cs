namespace IdentityServer.Application
{
    public interface ISecretListValidator
    {
        Task<ValidationResult> ValidateAsync(SecretValidationRequest request);
    }
}
