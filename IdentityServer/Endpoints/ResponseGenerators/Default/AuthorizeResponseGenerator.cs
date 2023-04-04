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

        public  Task<AuthorizeGeneratorResponse> ProcessAsync(AuthorizeGeneratorRequest request)
        {
            return Task.FromResult(new AuthorizeGeneratorResponse("", null,""));
        }
    }
}
