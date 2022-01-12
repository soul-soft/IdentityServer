using IdentityServer.Infrastructure;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using static IdentityServer.OpenIdConnects;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IServerUrl _urls;
        private readonly IIdGenerator _idGenerator;
        private readonly IReferenceTokenService _referenceTokenService;
        private readonly ITokenCreationService _tokenCreationService;

        public TokenService(
            IServerUrl urls,
            IIdGenerator idGenerator,
            ITokenCreationService tokenCreationService,
            IReferenceTokenService referenceTokenService)
        {
            _urls = urls;
            _idGenerator = idGenerator;
            _tokenCreationService = tokenCreationService;
            _referenceTokenService = referenceTokenService;
        }

        public Task<IToken> CreateAccessTokenAsync(TokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
            var resources = request.Resources;
            var token = new Token(id, issuer, TokenTypes.AccessToken, client.ClientId)
            {
                Nonce = request.Nonce,
                Scopes = request.Scopes,
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.AccessTokenLifetime,
                SubjectId = request.SubjectId,
            };
            foreach (var item in resources.ApiScopes)
            {
                token.Audiences.Add(item.Name);
            }
            if (client.IncludeJwtId)
            {
                token.Id = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex);
            }
            return Task.FromResult<IToken>(token);
        }

        public Task<IToken> CreateIdentityTokenAsync(TokenRequest request)
        {
            var id = _idGenerator.GeneratorId();
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
            var audiences = new List<string>();
            foreach (var item in request.Resources.ApiResources)
            {
                audiences.Add(item.Name);
            }
            var token = new Token(id, issuer, TokenTypes.IdentityToken, client.ClientId)
            {
                Nonce = request.Nonce,
                Scopes = request.Scopes,
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.IdentityTokenLifetime,
                SubjectId = request.SubjectId,
                Audiences = audiences
            };
            if (client.IncludeJwtId)
            {
                token.Id = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex);
            }
            return Task.FromResult<IToken>(token);
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
