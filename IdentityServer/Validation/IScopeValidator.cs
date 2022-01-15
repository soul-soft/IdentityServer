namespace IdentityServer.Validation
{
    public interface IScopeValidator
    {
        Task<ValidationResult> ValidateAsync(IEnumerable<string> allowedScopes,IEnumerable<string> requestedScopes);
    }
}
