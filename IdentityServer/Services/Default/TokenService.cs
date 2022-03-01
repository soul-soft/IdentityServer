using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IClaimsService _claimsService;
        private readonly IdentityServerOptions _options;
        private readonly IReferenceTokenStore _referenceTokenStore;
        private readonly ITokenCreationService _tokenCreationService;

        public TokenService(
            ISystemClock clock,
            IIdGenerator idGenerator,
            IClaimsService claimsService,
            IdentityServerOptions options,
            IReferenceTokenStore referenceTokenService,
            ITokenCreationService tokenCreationService)
        {
            _clock = clock;
            _options = options;
            _idGenerator = idGenerator;
            _claimsService = claimsService;
            _referenceTokenStore = referenceTokenService;
            _tokenCreationService = tokenCreationService;
        }

        public async Task<Token> CreateAccessTokenAsync(ValidatedTokenRequest request)
        {
            var id = await _idGenerator.GenerateAsync();
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, request.GrantType));
            var token = new Token
            {
                Id = id,
                Type = TokenTypes.AccessToken,
                Subject = subject,
                Lifetime = request.Client.AccessTokenLifetime,
                CreationTime = _clock.UtcNow.UtcDateTime,
                AccessTokenType = request.Client.AccessTokenType,
            };
            return token;
        }

        public async Task<Token> CreateIdentityTokenAsync(ValidatedTokenRequest request)
        {
            var id = await _idGenerator.GenerateAsync();
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, request.GrantType));
            var token = new Token
            {
                Id = id,
                Type = TokenTypes.AccessToken,
                Subject = subject,
                Lifetime = request.Client.AccessTokenLifetime,
                CreationTime = _clock.UtcNow.UtcDateTime,
                AccessTokenType = request.Client.AccessTokenType,
            };
            return token;
        }

        public async Task<string> CreateSecurityTokenAsync(Token token)
        {
            string securityToken;
            if (token.Type == TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    securityToken = await _tokenCreationService.CreateTokenAsync(token);
                }
                else
                {
                    var handle = await _referenceTokenStore.StoreReferenceTokenAsync(token);
                    securityToken = handle;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {

                securityToken = await _tokenCreationService.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
            return securityToken;
        }

    }
}
