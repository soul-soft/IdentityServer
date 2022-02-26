namespace IdentityServer.Validation
{
    public interface IScopeValidator
    {
        Task<ResourceCollection> ValidateAsync(IEnumerable<string> allowedScopes, IEnumerable<string> requestedScopes);
    }
}
