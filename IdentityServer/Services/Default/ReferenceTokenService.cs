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
            var referenceToken = new ReferenceToken(id, token, _clock.UtcNow.UtcDateTime);
            await _referenceTokens.SaveAsync(referenceToken);
            return id;
        }
    }
}
