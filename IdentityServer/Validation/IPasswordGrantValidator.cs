namespace IdentityServer.Validation
{
    public interface IPasswordGrantValidator
    {
        Task<ValidationResult> ValidateAsync(PasswordGrantContext context);
    }
}
