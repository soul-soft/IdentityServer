namespace IdentityServer.Application
{
    public interface ITokenRequestValidator
    {
        Task<ValidationResult> ValidateRequestAsync(TokenRequestValidationRequest request);
    }
}
