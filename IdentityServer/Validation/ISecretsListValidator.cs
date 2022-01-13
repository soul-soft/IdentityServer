namespace IdentityServer.Validation
{
    public interface ISecretsListValidator
    {
        Task<ValidationResult> ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets);
    }
}
