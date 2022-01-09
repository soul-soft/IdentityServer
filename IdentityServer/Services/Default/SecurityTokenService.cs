using System.IdentityModel.Tokens.Jwt;
using IdentityServer.Models;
using IdentityServer.Protocols;
using IdentityServer.Storage;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    public class SecurityTokenService : ISecurityTokenService
    {
        private readonly ISigningCredentialStore _credentials;

        public SecurityTokenService(ISigningCredentialStore credentials)
        {
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

            var header = new JwtHeader(credential);

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
            var payload = new JwtPayload();
            payload.AddClaims(token.Claims);
            return Task.FromResult(payload);
        }

        protected virtual Task<string> CreateJwtTokenAsync(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return Task.FromResult(handler.WriteToken(jwt));
        }
    }
}
