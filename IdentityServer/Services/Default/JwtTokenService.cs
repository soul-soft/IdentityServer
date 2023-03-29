using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialsStore _credentials;

        public JwtTokenService(
            IdentityServerOptions options,
            ISigningCredentialsStore credentials)
        {
            _options = options;
            _credentials = credentials;
        }

        public async Task<string> CreateTokenAsync(ReferenceToken token)
        {
            var credential = await _credentials
                    .GetSigningCredentialsByAlgorithmsAsync(token.AllowedSigningAlgorithms);
            var header = CreateJwtHeader(token, credential);
            var payload = CreateJwtPayload(token);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(new JwtSecurityToken(header, payload));
        }

        private JwtHeader CreateJwtHeader(ReferenceToken token, SigningCredentials credentials)
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

        private static JwtPayload CreateJwtPayload(ReferenceToken token)
        {
            return new JwtPayload(token.Claims);
        }
    }
}
