namespace IdentityServer.Validation
{
    public interface ISecretsValidator
    {
        Task<ValidationResult> ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets);
    }
}
