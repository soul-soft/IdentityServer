using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IdentityServerOptions _options;
        private readonly ISecurityTokenService _jwtTokenService;
        private readonly IReferenceTokenService _referenceTokenService;

        public TokenService(
            ISystemClock clock,
            IIdGenerator idGenerator,
            IdentityServerOptions options,
            ISecurityTokenService jwtTokenService,
            IReferenceTokenService referenceTokenService)
        {
            _clock = clock;
            _options = options;
            _idGenerator = idGenerator;
            _jwtTokenService = jwtTokenService;
            _referenceTokenService = referenceTokenService;
        }

        public Task<AccessToken> CreateAccessTokenAsync(ValidatedTokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _options.Issuer;
            var client = request.Client;
            var notBefore = _clock.UtcNow.UtcDateTime;
            var expiration = notBefore.AddSeconds(client.AccessTokenLifetime);
            var resources = request.Resources;
            var audiences = resources.ApiResources.Select(s => s.Name).ToList();
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
                Profiles = request.Subject.Claims.ToProfiles().ToList()
            };
            return Task.FromResult(token);
        }

        public Task<AccessToken> CreateIdentityTokenAsync(ValidatedTokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _options.Issuer;
            var client = request.Client;
            var resources = request.Resources;
            var notBefore = _clock.UtcNow.UtcDateTime;
            var authTime = _clock.UtcNow.UtcDateTime;
            var expiration = notBefore.AddSeconds(client.AccessTokenLifetime);
            var audiences = resources.ApiResources.Select(s => s.Name).ToList();
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
                Profiles = request.Subject.Claims.ToProfiles().ToList(),
            };
            return Task.FromResult(token);
        }

        public async Task<string> CreateSecurityTokenAsync(AccessToken token)
        {
            string tokenResult;
            if (token.Type == TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    tokenResult = await _jwtTokenService.CreateJwtTokenAsync(token);
                }
                else
                {
                    var handle = await _referenceTokenService.CreateReferenceTokenAsync(token);
                    tokenResult = handle;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {

                tokenResult = await _jwtTokenService.CreateJwtTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
            return tokenResult;
        }

    }
}
