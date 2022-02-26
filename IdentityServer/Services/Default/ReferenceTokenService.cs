using System.Security.Claims;
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

        public async Task<string> CreateReferenceTokenAsync(AccessToken token)
        {
            var id = _idGenerator.GeneratorId();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var referenceToken = new ReferenceToken(
                id, 
                token,
                token.Lifetime,
                creationTime);
            await _referenceTokens.AddAsync(referenceToken);
            return Base64UrlEncoder.Encode(id);
        }

        public async Task<ReferenceToken?> GetReferenceTokenAsync(string id)
        {
            id = Base64UrlEncoder.Decode(id);
            return await _referenceTokens.FindByIdAsync(id);
        }
    }
}
