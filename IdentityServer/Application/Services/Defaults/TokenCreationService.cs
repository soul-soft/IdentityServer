using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Application
{
    internal class TokenCreationService
        : ITokenCreationService
    {
        private readonly ILogger _logger;
        private readonly ISystemClock _clock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _signings;

        public TokenCreationService(
            ISystemClock clock,
            IdentityServerOptions options,
            ISigningCredentialStore signings,
            ILogger<TokenCreationService> logger)
        {
            _clock = clock;
            _logger = logger;
            _options = options;
            _signings = signings;
        }

        public async Task<string> CreateTokenAsync(Token token)
        {
            var header = await CreateHeaderAsync(token);
            var payload = await CreatePayloadAsync(token);
            var jwt = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }

        private async Task<JwtHeader> CreateHeaderAsync(Token token)
        {
            var signing = await _signings.GetSigningCredentialsAsync();

            if (signing == null)
            {
                throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
            }

            var header = new JwtHeader(signing);

            if (signing.Key is X509SecurityKey x509Key)
            {
                var cert = x509Key.Certificate;
                if (_clock.UtcNow.UtcDateTime > cert.NotAfter)
                {
                    _logger.LogWarning("Certificate {subjectName} has expired on {expiration}", cert.Subject, cert.NotAfter.ToString(CultureInfo.InvariantCulture));
                }
                header["x5t"] = Base64Url.Encode(cert.GetCertHash());
            }

            if (token.Type == OidcConstants.TokenTypes.AccessToken)
            {
                if (!string.IsNullOrWhiteSpace(_options.AccessTokenJwtType))
                {
                    header["typ"] = _options.AccessTokenJwtType;
                }
            }

            return header;
        }

        private Task<JwtPayload> CreatePayloadAsync(Token token)
        {
            var notbefore = _clock.UtcNow.UtcDateTime;
            DateTime? expires = null;
            if (token.Lifetime != null)
            {
                expires = notbefore.AddSeconds(token.Lifetime.Value);
            }
            var payload = new JwtPayload(token.Issuer, null, token.Claims, notbefore, expires);
            return Task.FromResult(payload);
        }
    }
}
