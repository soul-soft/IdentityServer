namespace IdentityServer.Validation
{
    public interface ISecretValidator
    {
        string SecretType { get; }
        Task ValidateAsync(ClientSecret clientSecret, IEnumerable<ISecret> allowedSecrets);
    }
}
