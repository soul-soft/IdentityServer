namespace IdentityServer.Services
{
    internal class ReferenceTokenService : IReferenceTokenService
    {
        private readonly IIdGenerator _idGenerator;
        private readonly IReferenceTokenStore _referenceTokens;

        public ReferenceTokenService(
            IIdGenerator idGenerator,
            IReferenceTokenStore referenceTokens)
        {
            _idGenerator = idGenerator;
            _referenceTokens = referenceTokens;
        }

        public async Task<string> CreateAsync(IToken token)
        {
            var id = _idGenerator.GeneratorId();
            var referenceToken = new ReferenceToken(id, token);
            await _referenceTokens.SaveAsync(referenceToken);
            return id;
        }
    }
}
