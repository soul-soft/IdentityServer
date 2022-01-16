namespace IdentityServer.Validation
{
    public interface ISecretsListValidator
    {
        Task ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets);
    }
}
