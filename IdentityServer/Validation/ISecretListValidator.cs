namespace IdentityServer.Validation
{
    public interface ISecretListValidator
    {
        Task ValidateAsync(ParsedSecret ClientCredentials, IEnumerable<Secret> allowedSecrets);
    }
}
