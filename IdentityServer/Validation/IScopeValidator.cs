namespace IdentityServer.Validation
{
    public interface IScopeValidator
    {
        Task ValidateAsync(IEnumerable<string> allowedScopes,IEnumerable<string> requestedScopes);
    }
}
