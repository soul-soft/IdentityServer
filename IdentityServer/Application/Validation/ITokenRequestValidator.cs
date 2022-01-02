using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public interface ITokenRequestValidator
    {
        Task<TokenRequestValidationResult> ValidateRequestAsync(HttpContext context, ClientSecretValidationResult clientValidationResult);
    }
}
