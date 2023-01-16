using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class AuthorizeCodeService : IAuthorizeCodeService
    {
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _generator;
        private readonly IAuthorizeCodeStore _store;

        public AuthorizeCodeService(
            ISystemClock clock,
            IIdGenerator generator,
            IAuthorizeCodeStore store
            )
        {
            _clock = clock;
            _store = store;
            _generator = generator;
        }

        public async Task<string> CreateAuthorizeCodeAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _generator.GenerateAsync(16);
            var creationTime = _clock.UtcNow.DateTime;
            var authorizeCode = new AuthorizeCode(id, client.AuthorizeCodeLifetime, subject.Claims, creationTime);
            await _store.StoreAuthorizeCodeAsync(authorizeCode);
            return authorizeCode.Id;
        }
    }
}
