namespace IdentityServer.Validation
{
    public interface IScopeValidator
    {
        Task<Resources> ValidateAsync(IClient client, IEnumerable<string> requestedScopes);
    }
}
