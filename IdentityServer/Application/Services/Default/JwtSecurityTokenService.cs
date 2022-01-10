using IdentityServer.Models;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class JwtSecurityTokenService : ISecurityTokenService
    {
        private readonly ISystemClock _clock;
        private readonly ISigningCredentialStore _credentials;

        public JwtSecurityTokenService(
            ISystemClock clock,
            ISigningCredentialStore credentials)
        {
            _clock=clock;
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
            if (request.Type == OpenIdConnectConstants.TokenTypes.AccessToken)
            {
                header["typ"] = "at+jwt";
            }
            return header;
        }

        protected Task<JwtPayload> CreateJwtPayloadAsync(IToken token)
        {
            var notBefore = _clock.UtcNow.UtcDateTime;
            DateTime? expires = null;
            if (token.Lifetime.HasValue)
            {
                expires = notBefore.AddSeconds(token.Lifetime.Value);
            }
            var issuerAt = _clock.UtcNow.ToUnixTimeSeconds().ToString();
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(token.SessionId))
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, token.SessionId));
            }
            if (!string.IsNullOrEmpty(token.SessionId))
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, token.SessionId));
            }
            foreach (var item in token.Audiences)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, item));
            }
            var payload = new JwtPayload(
                issuer: token.Issuer,
                audience: null,
                claims: claims,
                notBefore: notBefore,
                expires: expires);
            return Task.FromResult(payload);
        }

        protected Task<string> CreateJwtTokenAsync(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return Task.FromResult(handler.WriteToken(jwt));
        }
    }
}
