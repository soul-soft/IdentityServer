using IdentityModel;
using IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class PostBodySecretParser
        : ISecretParser
    {
        private readonly ILogger _logger;

        private readonly IdentityServerOptions _options;

        public PostBodySecretParser(
            ILogger<PostBodySecretParser> logger,
            IdentityServerOptions options)
        {
            _logger = logger;
            _options = options;
        }

        public string AuthenticationMethod => OidcConstants.EndpointAuthenticationMethods.PostBody;

        public async Task<ParsedSecret?> ParseAsync(HttpContext context)
        {
            _logger.LogDebug("Start parsing for secret in post body");

            if (!context.Request.HasApplicationFormContentType())
            {
                _logger.LogDebug("Content type is not a form");
                return null;
            }

            var body = await context.Request.ReadFormAsync();
            var clientId = body["client_id"].FirstOrDefault();
            var secret = body["client_secret"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(clientId))
            {
                _logger.LogError("No clientId in post body found");
                return null;
            }

            if (clientId.Length > _options.InputLengthRestrictions.ClientId)
            {
                _logger.LogError("Client ID exceeds maximum length.");
                return null;
            }

            if (!string.IsNullOrWhiteSpace(secret))
            {
                if (secret.Length > _options.InputLengthRestrictions.ClientSecret)
                {
                    _logger.LogError("Client secret exceeds maximum length.");
                    return null;
                }

                return new ParsedSecret(clientId, secret, IdentityServerConstants.ParsedSecretTypes.SharedSecret); ;
            }
            else
            {
                // client secret is optional
                _logger.LogDebug("client id without secret found");

                return new ParsedSecret(clientId, IdentityServerConstants.ParsedSecretTypes.NoSecret);
            }
        }
    }
}
