namespace IdentityServer.Validation
{
    public interface IAuthorizeCodeRequestValidator
    {
        Task<AuthorizeCodeValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request);
    }
}
