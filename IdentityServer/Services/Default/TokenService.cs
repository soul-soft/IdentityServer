using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IServerUrl _urls;
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IClaimsService _claimsService;
        private readonly ISecurityTokenService _jwtTokenService;
        private readonly IReferenceTokenService _referenceTokenService;

        public TokenService(
            IServerUrl urls,
            ISystemClock clock,
            IIdGenerator idGenerator,
            IClaimsService claimsService,
            ISecurityTokenService jwtTokenService,
            IReferenceTokenService referenceTokenService)
        {
            _urls = urls;
            _clock = clock;
            _idGenerator = idGenerator;
            _claimsService = claimsService;
            _jwtTokenService = jwtTokenService;
            _referenceTokenService = referenceTokenService;
        }

        public async Task<IAccessToken> CreateAccessTokenAsync(ValidatedTokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _urls.GetIssuerUri();
            var client = request.Client;
            var resources = request.Resources;
            var notBefore = _clock.UtcNow.UtcDateTime;
            var authTime = _clock.UtcNow.UtcDateTime;
            var expiration = notBefore.AddSeconds(client.AccessTokenLifetime);
            var audiences = resources.ApiResources.Select(s => s.Name).ToList();
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request);
            var claimLites = claims.ToClaimLites().ToArray();
            var subjectId = request.Subject.GetSubjectId();
            var token = new AccessToken
            {
                Id = id,
                Type = TokenTypes.AccessToken,
                Issuer = issuer,
                ClientId = client.ClientId,
                GrantType = request.GrantType,
                Nonce = request.Nonce,
                Scopes = request.Scopes.ToArray(),
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.AccessTokenLifetime,
                SubjectId = subjectId,
                Audiences = audiences,
                NotBefore = notBefore,
                Expiration = expiration,
                Claims = claimLites,
            };
            return token;
        }

        public async Task<IAccessToken> CreateIdentityTokenAsync(ValidatedTokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _urls.GetIssuerUri();
            var client = request.Client;
            var resources = request.Resources;
            var notBefore = _clock.UtcNow.UtcDateTime;
            var authTime = _clock.UtcNow.UtcDateTime;
            var expiration = notBefore.AddSeconds(client.AccessTokenLifetime);
            var audiences = resources.ApiResources.Select(s => s.Name).ToList();
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request);
            var claimLites = claims.ToClaimLites().ToArray();
            var subjectId = request.Subject.GetSubjectId();
            var token = new AccessToken
            {
                Id = id,
                Type = TokenTypes.AccessToken,
                Issuer = issuer,
                ClientId = client.ClientId,
                GrantType = request.GrantType,
                Nonce = request.Nonce,
                Scopes = request.Scopes.ToArray(),
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.AccessTokenLifetime,
                SubjectId = subjectId,
                Audiences = audiences,
                NotBefore = notBefore,
                Expiration = expiration,
                Claims = claimLites,
            };
            return token;
        }

        public async Task<string> CreateSecurityTokenAsync(IAccessToken token)
        {
            string tokenResult;
            if (token.Type == TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    tokenResult = await _jwtTokenService.CreateAsync(token);
                }
                else
                {
                    var handle = await _referenceTokenService.CreateAsync(token);
                    tokenResult = handle;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {

                tokenResult = await _jwtTokenService.CreateAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
            return tokenResult;
        }

    }
}
