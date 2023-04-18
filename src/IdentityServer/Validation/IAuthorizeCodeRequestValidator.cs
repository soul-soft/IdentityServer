namespace IdentityServer.Validation
{
    public interface IAuthorizeCodeRequestValidator
    {
        Task<GrantValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request);
    }
}
