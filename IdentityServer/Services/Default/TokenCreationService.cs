using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    internal class TokenCreationService : ISecurityTokenService
    {
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialsStore _credentials;

        public TokenCreationService(
            IdentityServerOptions options,
            ISigningCredentialsStore credentials)
        {
            _options = options;
            _credentials = credentials;
        }

        public async Task<string> CreateTokenAsync(Token token)
        {
            var credential = await _credentials
                    .GetSigningCredentialsByAlgorithmsAsync(Array.Empty<string>());
            var header = CreateJwtHeader(token, credential);
            var payload = CreateJwtPayload(token);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(new JwtSecurityToken(header, payload));
        }

        private JwtHeader CreateJwtHeader(Token token, SigningCredentials credentials)
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

        private JwtPayload CreateJwtPayload(Token token)
        {
            var claims = new List<Claim>();
            if (token.Subject != null)
            {
                claims.AddRange(token.Subject.Claims);
            }
            var time = token.CreationTime;
            var now = new DateTimeOffset(time).ToUnixTimeSeconds();
            var exp = now + token.Lifetime;
            claims.Add(new Claim(JwtClaimTypes.JwtId, token.Id));
            claims.Add(new Claim(JwtClaimTypes.NotBefore, now.ToString()));
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, now.ToString()));
            claims.Add(new Claim(JwtClaimTypes.Expiration, exp.ToString()));
            var payload = new JwtPayload(claims);
            return payload;
        }
    }
}
