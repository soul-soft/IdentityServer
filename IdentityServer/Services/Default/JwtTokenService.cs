using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    internal class JwtTokenService : ISecurityTokenService
    {
        private readonly IdentityServerOptions _options;

        private readonly ISigningCredentialStore _credentials;

        public JwtTokenService(
            IdentityServerOptions options,
            ISigningCredentialStore credentials)
        {
            _options = options;
            _credentials = credentials;
        }

        public async Task<string> CreateAsync(IAccessToken token)
        {
            var credential = await _credentials
              .GetSigningCredentialsByAlgorithmsAsync(token.AllowedSigningAlgorithms);
            var header = CreateJwtHeader(token, credential);
            var payload = CreateJwtPayload(token);
            return CreateJwtToken(new JwtSecurityToken(header, payload));
        }

        private JwtHeader CreateJwtHeader(IAccessToken token, SigningCredentials credentials)
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

        private JwtPayload CreateJwtPayload(IAccessToken token)
        {
            var claims = token.ToClaims(_options);
            var payload = new JwtPayload(claims);
            return payload;
        }

        private string CreateJwtToken(JwtSecurityToken jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }
    }
}
