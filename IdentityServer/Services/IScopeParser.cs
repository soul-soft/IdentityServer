namespace IdentityServer.Validation
{
    public interface IScopeParser
    {
        Task<IEnumerable<string>> ParseAsync(IEnumerable<string> scopes);
    }
}
