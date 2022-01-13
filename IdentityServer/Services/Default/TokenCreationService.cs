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
            var credential = await _credentials
              .GetSigningCredentialsByAlgorithmsAsync(token.AllowedSigningAlgorithms);
            var header = CreateJwtHeader(token, credential);
            var payload = CreateJwtPayload(token);
            return CreateJwtToken(new JwtSecurityToken(header, payload));
        }

        private JwtHeader CreateJwtHeader(IToken token, SigningCredentials credentials)
        {
            var header = new JwtHeader(credentials);
            if (credentials.Key is X509SecurityKey x509Key)
            {
                var cert = x509Key.Certificate;
                header["x5t"] = Base64UrlEncoder.Encode(cert.GetCertHash());
            }
            if (token.Type == TokenTypes.AccessToken)
            {
                if (!string.IsNullOrWhiteSpace(_options.AccessTokenJwtType))
                {
                    header["typ"] = _options.AccessTokenJwtType;
                }
            }
            return header;
        }

        private JwtPayload CreateJwtPayload(IToken token)
        {
            var notBefore = _clock.UtcNow.UtcDateTime;
            DateTime? expires = notBefore.AddSeconds(token.Lifetime);
            var payload = new JwtPayload(
                issuer: token.Issuer,
                audience: null,
                claims: null,
                notBefore: notBefore,
                expires: expires);
            payload.Add(JwtClaimTypes.JwtId, token.Id);
            payload.Add(JwtClaimTypes.Audience, token.Audiences);
            if (string.IsNullOrWhiteSpace(token.ClientId))
            {
                payload.Add(JwtClaimTypes.ClientId, token.ClientId);
            }
            if (string.IsNullOrWhiteSpace(token.GrantType))
            {
                payload.Add(JwtClaimTypes.AuthenticationMethod, token.GrantType);
            }
            if (string.IsNullOrWhiteSpace(_options.IdentityProvider))
            {
                payload.Add(JwtClaimTypes.IdentityProvider, _options.IdentityProvider);
            }
            if (!string.IsNullOrWhiteSpace(token.SubjectId))
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
            payload.AddClaims(token.Claims.ToClaims());
            return payload;
        }

        private string CreateJwtToken(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }
    }
}
