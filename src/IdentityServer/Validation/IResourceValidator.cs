namespace IdentityServer.Validation
{
    public interface IResourceValidator
    {
        Task<Resources> ValidateAsync(Client client, IEnumerable<string> scopes);
    }
}
