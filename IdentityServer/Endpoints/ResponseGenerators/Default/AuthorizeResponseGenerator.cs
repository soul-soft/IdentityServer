using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeResponseGenerator : IAuthorizeResponseGenerator
    {
        private readonly IAuthorizeCodeService _authorizeCodeService;
        
        public AuthorizeResponseGenerator(
            IAuthorizeCodeService authorizeCodeService)
        {
            _authorizeCodeService = authorizeCodeService;
        }
     
        public async Task<AuthorizeGeneratorResponse> ProcessAsync(AuthorizeGeneratorRequest request)
        {
            var code = await _authorizeCodeService.CreateAuthorizeCodeAsync(request.Client, request.Subject);
            return new AuthorizeGeneratorResponse(code, request.RedirectUri);
        }
    }
}
