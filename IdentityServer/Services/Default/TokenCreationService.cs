using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer.Services
{
    internal class TokenCreationService : ITokenCreationService
    {
        private readonly ISystemClock _clock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;

        public TokenCreationService(
            ISystemClock clock,
            IdentityServerOptions options,
            ISigningCredentialStore credentials)
        {
            _clock = clock;
            _options = options;
            _credentials = credentials;
        }

        public async Task<string> CreateTokenAsync(IToken token)
        {
            var header = await CreateJwtHeaderAsync(token);
            var payload = await CreateJwtPayloadAsync(token);
            return await CreateJwtTokenAsync(new JwtSecurityToken(header, payload));
        }

        private async Task<JwtHeader> CreateJwtHeaderAsync(IToken request)
        {
            var credential = await _credentials
                .GetSigningCredentialsByAlgorithmsAsync(request.AllowedSigningAlgorithms);

            var header = new JwtHeader(credential);

            if (credential.Key is X509SecurityKey x509Key)
            {
                var cert = x509Key.Certificate;
                header["x5t"] = Base64UrlEncoder.Encode(cert.GetCertHash());
            }
            if (request.Type == OpenIdConnects.TokenTypes.AccessToken)
            {
                if (!string.IsNullOrWhiteSpace(_options.AccessTokenJwtType))
                {
                    header["typ"] = _options.AccessTokenJwtType;
                }
            }
            return header;
        }

        protected Task<JwtPayload> CreateJwtPayloadAsync(IToken token)
        {
            var notBefore = _clock.UtcNow.UtcDateTime;
            DateTime? expires = notBefore.AddSeconds(token.Lifetime);
            var payload = new JwtPayload(
                issuer: token.Issuer,
                audience: null,
                claims: null,
                notBefore: notBefore,
                expires: expires);
            payload.Add(JwtClaimTypes.ClientId, token.ClientId);
            payload.Add(JwtClaimTypes.JwtId, token.Id);
            payload.Add(JwtClaimTypes.Audience, token.Audiences);
            if (!string.IsNullOrEmpty(token.SubjectId))
            {
                payload.Add(JwtClaimTypes.Subject, token.SubjectId);
            }
            if (!string.IsNullOrEmpty(token.SessionId))
            {
                payload.Add(JwtClaimTypes.SessionId, token.SessionId);
            }
            if (_options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                payload.Add(JwtClaimTypes.Scope, string.Join(",", token.Scopes));
            }
            else
            {
                payload.Add(JwtClaimTypes.Scope, token.Scopes);
            }
            return Task.FromResult(payload);
        }

        protected Task<string> CreateJwtTokenAsync(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return Task.FromResult(handler.WriteToken(jwt));
        }
    }
}
