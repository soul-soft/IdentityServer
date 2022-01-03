namespace IdentityServer.Application
{
    public interface IScopeValidator
    {
        Task<ValidationResult> ValidateAsync(ScopeValidationRequest request);
    }
}
