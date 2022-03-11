using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public class PostBodySecretParser : ISecretParser
    {

        private readonly IdentityServerOptions _options;

        public PostBodySecretParser(IdentityServerOptions options)
        {
            _options = options;
        }

        public string AuthenticationMethod => TokenEndpointAuthMethods.PostBody;

        public async Task<ParsedCredentials> ParseAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            var clientId = form["client_id"].FirstOrDefault();
            var credentials = form["client_secret"].FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidRequest,"Client ID is missing.");
            }
            if (clientId.Length > _options.InputLengthRestrictions.ClientId)
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Client ID is too long.");
            }
            if (credentials.Length > _options.InputLengthRestrictions.ClientSecret)
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidRequest, "Client secret is too long.");
            }
            if (string.IsNullOrEmpty(credentials))
            {
                return new ParsedCredentials(clientId, credentials, ClientSecretTypes.NoSecret);
            }
            else
            {
                return new ParsedCredentials(clientId, credentials, ClientSecretTypes.SharedSecret);
            }
        }
    }
}
