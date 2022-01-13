using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IServerUrl _urls;
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IClaimIssueService _claimsService;
        private readonly IReferenceTokenService _referenceTokenService;
        private readonly ISecurityTokenService _tokenCreationService;

        public TokenService(
            IServerUrl urls,
            ISystemClock clock,
            IIdGenerator idGenerator,
            IClaimIssueService claimsService,
            ISecurityTokenService tokenCreationService,
            IReferenceTokenService referenceTokenService)
        {
            _urls = urls;
            _clock = clock;
            _idGenerator = idGenerator;
            _claimsService = claimsService;
            _tokenCreationService = tokenCreationService;
            _referenceTokenService = referenceTokenService;
        }

        public async Task<IToken> CreateAccessTokenAsync(TokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
            var resources = request.Resources;
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request);
            var token = new Token(id, TokenTypes.AccessToken)
            {
                Issuer = issuer,
                ClientId = client.ClientId,
                GrantType = request.GrantType,
                Nonce = request.Nonce,
                Scopes = request.Scopes,
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.AccessTokenLifetime,
                SubjectId = request.SubjectId,
                CreationTime = _clock.UtcNow.UtcDateTime
            };
            foreach (var item in claims)
            {
                token.Claims.Add(new ClaimLite(item.Type, item.Value, item.ValueType));
            }
            foreach (var item in resources.ApiResources)
            {
                token.Audiences.Add(item.Name);
            }
            return token;
        }

        public async Task<IToken> CreateIdentityTokenAsync(TokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
            var audiences = new List<string>();
            foreach (var item in request.Resources.ApiResources)
            {
                audiences.Add(item.Name);
            }
            var token = new Token(id, TokenTypes.AccessToken)
            {
                Issuer = issuer,
                ClientId = client.ClientId,
                GrantType = request.GrantType,
                Nonce = request.Nonce,
                Scopes = request.Scopes,
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.AccessTokenLifetime,
                SubjectId = request.SubjectId,
                CreationTime = _clock.UtcNow.UtcDateTime
            };
            var claims = await _claimsService.GetIdentityTokenClaimsAsync(request);
            foreach (var item in claims)
            {
                token.Claims.Add(new ClaimLite(item.Type, item.Value, item.ValueType));
            }
            return token;
        }

        public async Task<string> CreateSecurityTokenAsync(IToken token)
        {
            string tokenResult;
            if (token.Type == TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    tokenResult = await _tokenCreationService.CreateTokenAsync(token);
                }
                else
                {
                    var handle = await _referenceTokenService.CreateAsync(token);
                    tokenResult = handle;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {

                tokenResult = await _tokenCreationService.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
            return tokenResult;
        }
    }
}
