using IdentityServer.Models;
using IdentityServer.Storage;

namespace IdentityServer.Services
{
    internal class RefreshTokenService : IRefreshTokenService
    {
        private readonly IIdGenerator _idGenerator;
        private readonly IRefreshTokenStore _refreshTokenStore;
      
        public RefreshTokenService(
            IIdGenerator idGenerator,
            IRefreshTokenStore refreshTokenStore)
        {
            _idGenerator = idGenerator;
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task<string> CreateAsync(IToken token,int lifetime)
        {
            var id = _idGenerator.GeneratorId();
            var refreshToken = new RefreshToken(id, token, lifetime);
            await _refreshTokenStore.SaveAsync(refreshToken);
            return id;
        }
    }
}
