using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<string> CreateReferenceTokenAsync(IToken token)
        {
            var id = _idGenerator.GeneratorId();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var referenceToken = new ReferenceToken(
                id, 
                token,
                token.Lifetime,
                creationTime);
            await _referenceTokens.SaveAsync(referenceToken);
            return Base64UrlEncoder.Encode(id);
        }
    }
}
