using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Services
{
    internal class ReferenceTokenService : IReferenceTokenService
    {
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IReferenceTokenStore _referenceTokens;

        public ReferenceTokenService(
            ISystemClock clock,
            IIdGenerator idGenerator,
            IReferenceTokenStore referenceTokens)
        {
            _clock = clock;
            _idGenerator = idGenerator;
            _referenceTokens = referenceTokens;
        }

        public async Task<string> CreateAsync(IToken token)
        {
            var id = _idGenerator.GeneratorId();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var expiration = creationTime.AddSeconds(token.Lifetime);
            var referenceToken = new ReferenceToken(
                id, 
                token,
                token.Lifetime,
                creationTime);
            await _referenceTokens.SaveAsync(referenceToken);
            return id;
        }
    }
}
