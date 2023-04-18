namespace IdentityServer.Validation
{
    public interface IClientCredentialsRequestValidator
    {
        Task<GrantValidationResult> ValidateAsync(ClientCredentialsValidationRequest request);
    }
}
