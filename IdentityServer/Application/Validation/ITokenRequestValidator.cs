namespace IdentityServer.Application
{
    public interface ITokenRequestValidator
    {
        Task ValidateRequestAsync(TokenRequestValidationContext context);
    }
}
