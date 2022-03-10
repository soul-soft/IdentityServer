namespace IdentityServer.Validation
{
    public interface ISecretValidator
    {
        string CredentialsType { get; }
        Task ValidateAsync(ClientCredentials clientCredentials, IEnumerable<Secret> allowedSecrets);
    }
}
