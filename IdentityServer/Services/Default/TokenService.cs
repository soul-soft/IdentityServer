using IdentityServer.Infrastructure;
using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;
using static IdentityServer.Protocols.OpenIdConnectConstants;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IServerUrl _urls;
        private readonly ISystemClock _clock;
        private readonly ITokenCreationService _securityToken;
        private readonly IReferenceTokenStore _referenceTokens;

        public TokenService(
            IServerUrl urls,
            ISystemClock clock,
            IReferenceTokenStore referenceTokens,
            ITokenCreationService securityToken,
            ISigningCredentialStore credentials)
        {
            _urls = urls;
            _clock = clock;
            _referenceTokens = referenceTokens;
            _securityToken = securityToken;
        }

        public Task<IToken> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
            var resources = request.Resources;
            var token = new Token(issuer, TokenTypes.AccessToken, client.ClientId)
            {
                Nonce = request.Nonce,
                Scopes = request.Scopes,
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.AccessTokenLifetime,
                SubjectId = request.SubjectId,
                CreationTime = _clock.UtcNow.UtcDateTime,
            };
            foreach (var item in resources.ApiScopes)
            {
                token.Audiences.Add(item.Name);
            }
            if (client.IncludeJwtId)
            {
                token.JwtId = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex);
            }
            return Task.FromResult<IToken>(token);
        }

        public Task<IToken> CreateIdentityTokenAsync(TokenCreationRequest request)
        {
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
            var audiences = new List<string>();
            foreach (var item in request.Resources.ApiResources)
            {
                audiences.Add(item.Name);
            }
            var token = new Token(issuer, TokenTypes.IdentityToken, client.ClientId)
            {
                Nonce = request.Nonce,
                Scopes = request.Scopes,
                SessionId = request.SessionId,
                AccessTokenType = client.AccessTokenType,
                Description = request.Description,
                Lifetime = client.IdentityTokenLifetime,
                SubjectId = request.SubjectId,
                CreationTime = _clock.UtcNow.UtcDateTime,
                Audiences = audiences
            };
            if (client.IncludeJwtId)
            {
                token.JwtId = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex);
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
                    tokenResult = await _securityToken.CreateTokenAsync(token);
                }
                else
                {
                    var handle = await _referenceTokens.SaveAsync(token);

                    tokenResult = handle;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {

                tokenResult = await _securityToken.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }

            return tokenResult;
        }


    }
}
