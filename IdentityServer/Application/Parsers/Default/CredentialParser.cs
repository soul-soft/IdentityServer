using IdentityModel;
using IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class CredentialParser : ICredentialParser
    {
        private readonly IdentityServerOptions _options;

        public CredentialParser(IdentityServerOptions options)
        {
            _options = options;
        }

        public string AuthenticationMethod => OidcConstants.EndpointAuthenticationMethods.PostBody;

        public async Task<ParsedCredential> ParseAsync(HttpContext context)
        {
            if (!context.Request.HasApplicationFormContentType())
            {
                throw new InvalidOperationException("Content type is not a form");
            }

            var body = await context.Request.ReadFormAsync();
            var clientId = body[OidcConstants.TokenRequest.ClientId].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new InvalidOperationException("No clientId in post body found");
            }
            if (clientId.Length > _options.InputLengthRestrictions.ClientId)
            {
                throw new InvalidOperationException("Client ID exceeds maximum length.");
            }

            var clientSecret = body[OidcConstants.TokenRequest.ClientSecret].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(clientSecret))
            {
                if (clientSecret.Length > _options.InputLengthRestrictions.ClientSecret)
                {
                    throw new InvalidOperationException("Client secret exceeds maximum length.");
                }

                return new ParsedCredential(clientId, clientSecret, IdentityServerConstants.ParsedSecretTypes.SharedSecret);
            }
            else
            {
                return new ParsedCredential(clientId, IdentityServerConstants.ParsedSecretTypes.NoSecret);
            }
        }
    }
}
