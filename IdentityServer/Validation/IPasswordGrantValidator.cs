namespace IdentityServer.Validation
{
    public interface IPasswordGrantValidator
    {
        Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantValidationContext context);
    }
}
