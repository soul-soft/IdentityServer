﻿using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class AuthorizeCodeService : IAuthorizeCodeService
    {
        private readonly ISystemClock _clock;
        private readonly IAuthorizeCodeStore _store;
        private readonly IRandomGenerator _randomGenerator;

        public AuthorizeCodeService(
            ISystemClock clock,
            IAuthorizeCodeStore store,
            IRandomGenerator randomGenerator)
        {
            _clock = clock;
            _store = store;
            _randomGenerator = randomGenerator;
        }

        public async Task<string> CreateAuthorizeCodeAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _randomGenerator.GenerateAsync(16);
            var creationTime = _clock.UtcNow.DateTime;
            var authorizeCode = new AuthorizeCode(id, client.AuthorizeCodeLifetime, subject.Claims, creationTime);
            await _store.StoreAuthorizeCodeAsync(authorizeCode);
            return authorizeCode.Id;
        }
    }
}
