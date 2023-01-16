using IdentityServer.Validation;
using Microsoft.Extensions.Caching.Distributed;

namespace Hosting.Configuration
{
    public class AuthorizeCodeRequestValidator : IAuthorizeCodeRequestValidator
    {
        private readonly IDistributedCache _cache;
        public AuthorizeCodeRequestValidator(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<AuthorizeCodeValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request)
        {
            var userId = await _cache.GetStringAsync(request.Code);
            var result = new AuthorizeCodeValidationResult(userId);
            return result;
        }
    }
}
