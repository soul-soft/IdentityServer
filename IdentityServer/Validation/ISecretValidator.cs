namespace IdentityServer.Validation
{
    public interface ISecretValidator
    {
        string CredentialsType { get; }
        Task ValidateAsync(ParsedCredentials clientCredentials, IEnumerable<Secret> allowedSecrets);
    }
}
