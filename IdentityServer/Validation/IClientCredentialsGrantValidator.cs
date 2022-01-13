namespace IdentityServer.Validation
{
    public interface IClientCredentialsGrantValidator
    {
        Task<GrantValidationResult> ValidateAsync(ClientCredentialsGrantValidationContext context);
    }
}
