namespace IdentityServer.Validation
{
    public interface IClientCredentialsRequestValidator
    {
        Task<ClientCredentialsValidationResult> ValidateAsync(ClientCredentialsValidationRequest request);
    }
}
