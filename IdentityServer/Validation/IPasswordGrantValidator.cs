namespace IdentityServer.Validation
{
    public interface IPasswordGrantValidator
    {
        Task ValidateAsync(PasswordGrantValidationRequest context);
    }
}
