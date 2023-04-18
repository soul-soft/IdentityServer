namespace IdentityServer.Validation
{
    public interface ISecretValidator
    {
        string CredentialsType { get; }
        Task ValidateAsync(ParsedSecret clientCredentials, IEnumerable<Secret> allowedSecrets);
    }
}
