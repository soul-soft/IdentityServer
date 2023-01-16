using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class AuthorizeCodeService : IAuthorizeCodeService
    {
        private readonly IAuthorizeCodeStore _store;
        private readonly IIdGenerator _generator;

        public AuthorizeCodeService(
            IAuthorizeCodeStore store,
            IIdGenerator generator)
        {
            _store = store;
            _generator = generator;
        }

        public async Task<string> CreateAuthorizeCodeAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _generator.GenerateAsync(16);
            var authorizeCode = new AuthorizeCode(id, client.AuthorizeCodeLifetime, subject.Claims);
            await _store.StoreAuthorizeCodeAsync(authorizeCode);
            return authorizeCode.Id;
        }
    }
}
