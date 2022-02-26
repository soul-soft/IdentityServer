namespace IdentityServer.Validation
{
    public interface IClientCredentialsValidator
    {
        string CredentialsType { get; }
        Task ValidateAsync(ClientCredentials clientCredentials, IEnumerable<Secret> allowedSecrets);
    }
}
