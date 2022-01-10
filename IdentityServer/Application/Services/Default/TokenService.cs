using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityServer.Infrastructure;
using IdentityServer.Models;
using IdentityServer.Parsers;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using static IdentityServer.Protocols.OpenIdConnectConstants;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IUrlParse _urls;
        private readonly ISystemClock _clock;
        private readonly ISecurityTokenService _securityToken;
        private readonly IReferenceTokenStore _referenceTokens;
        private readonly ISigningCredentialStore _credentials;

        public TokenService(
            IUrlParse urls,
            ISystemClock clock,
            IReferenceTokenStore referenceTokens,
            ISecurityTokenService securityToken,
            ISigningCredentialStore credentials)
        {
            _urls = urls;
            _clock = clock;
            _referenceTokens = referenceTokens;
            _securityToken = securityToken;
            _credentials = credentials;
        }

        public Task<IToken> CreateAccessTokenAsync(TokenRequest request)
        {
            var issuer = _urls.GetIdentityServerIssuerUri();
            var client = request.Client;
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
            if (client.IncludeJwtId)
            {
                token.JwtId = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex);
            }
            return Task.FromResult<IToken>(token);
        }

        public Task<IToken> CreateIdentityTokenAsync(TokenRequest request)
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
