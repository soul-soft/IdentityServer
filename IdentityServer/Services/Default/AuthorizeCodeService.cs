using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class AuthorizeCodeService : IAuthorizeCodeService
    {
        private readonly ISystemClock _clock;
        private readonly IAuthorizationCodeStore _store;
        private readonly IRandomGenerator _randomGenerator;

        public AuthorizeCodeService(
            ISystemClock clock,
            IAuthorizationCodeStore store,
            IRandomGenerator randomGenerator)
        {
            _clock = clock;
            _store = store;
            _randomGenerator = randomGenerator;
        }

        public async Task<string> GenerateCodeAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var lifetime = client.AuthorizeCodeLifetime;
            var expirationTime = creationTime.AddSeconds(lifetime);
            var code = new AuthorizationCode(
                code: id,
                lifetime: client.AuthorizeCodeLifetime,
                claims: subject.Claims.ToArray(),
                expirationTime: expirationTime,
                creationTime);
            await _store.SaveAuthorizationCodeAsync(code);
            return code.Code;
        }
    }
}
