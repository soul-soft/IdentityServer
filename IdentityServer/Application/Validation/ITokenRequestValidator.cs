using System.Collections.Specialized;

namespace IdentityServer.Application
{
    public interface ITokenRequestValidator
    {
        Task<TokenRequestValidationResult> ValidateRequestAsync(NameValueCollection parameters, ClientSecretValidationResult result);
    }
}
