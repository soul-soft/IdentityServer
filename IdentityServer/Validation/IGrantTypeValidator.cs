namespace IdentityServer.Validation
{
    public interface IGrantTypeValidator
    {
        Task<ValidationResult> ValidateAsync(string requestedGrantType, IEnumerable<string> allowedGrantTypes);
    }
}
