using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityServer.Models;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    public class SecurityTokenService : ISecurityTokenService
    {
        private readonly ISystemClock _clock;
        private readonly ISigningCredentialStore _credentials;

        public SecurityTokenService(
            ISystemClock clock,
            ISigningCredentialStore credentials)
        {
            _clock = clock;
            _credentials = credentials;
        }

        public async Task<string> CreateAccessTokenAsync(SecurityTokenRequest request)
        {
            var header = await CreateJwtHeaderAsync(request);
            var payload = await CreateJwtPayloadAsync(request);
            return await CreateJwtTokenAsync(new JwtSecurityToken(header, payload));
        }

        protected virtual async Task<JwtHeader> CreateJwtHeaderAsync(SecurityTokenRequest request)
        {
            var credential = await _credentials
                .GetSigningCredentialsByAlgorithmsAsync(request.AllowedSigningAlgorithm);

            var header = new JwtHeader(credential.SigningCredentials);

            if (credential.Key is X509SecurityKey x509Key)
            {
                var cert = x509Key.Certificate;
                header["x5t"] = Base64UrlEncoder.Encode(cert.GetCertHash());
            }
            if (request.TokenType == OpenIdConnectTokenType.AccessToken)
            {
                header["typ"] = "at+jwt";
            }
            return header;
        }

        protected virtual Task<JwtPayload> CreateJwtPayloadAsync(SecurityTokenRequest token)
        {
            var notBefore = _clock.UtcNow.UtcDateTime;
            DateTime? expires=null;
            if (token.Lifetime.HasValue)
            {
                expires = notBefore.AddSeconds(token.Lifetime.Value);
            }
            var  issuerAt = _clock.UtcNow.ToUnixTimeSeconds().ToString();
            token.Claims.Add(new Claim(OpenIdConnectClaimTypes.Issuer, issuerAt, ClaimValueTypes.Integer64));
            var payload = new JwtPayload(
                issuer: token.Issuer,
                audience: null,
                claims: token.Claims,
                notBefore: notBefore,
                expires: expires);
            return Task.FromResult(payload);
        }

        protected virtual Task<string> CreateJwtTokenAsync(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return Task.FromResult(handler.WriteToken(jwt));
        }
    }
}
