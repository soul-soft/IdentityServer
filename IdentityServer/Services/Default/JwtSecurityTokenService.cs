using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    internal class JwtSecurityTokenService : ISecurityTokenService
    {
        private readonly IdentityServerOptions _options;

        private readonly ISigningCredentialsStore _credentials;

        public JwtSecurityTokenService(
            IdentityServerOptions options,
            ISigningCredentialsStore credentials)
        {
            _options = options;
            _credentials = credentials;
        }

        public async Task<string> CreateJwtTokenAsync(AccessToken token)
        {
            var credential = await _credentials
              .GetSigningCredentialsByAlgorithmsAsync(token.AllowedSigningAlgorithms);
            var header = CreateJwtHeader(token, credential);
            var payload = CreateJwtPayload(token);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(new JwtSecurityToken(header, payload));
        }

        private JwtHeader CreateJwtHeader(AccessToken token, SigningCredentials credentials)
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

        private JwtPayload CreateJwtPayload(AccessToken token)
        {
            var claims = token.ToClaims(_options);
            var payload = new JwtPayload(claims);
            return payload;
        }
    }
}
