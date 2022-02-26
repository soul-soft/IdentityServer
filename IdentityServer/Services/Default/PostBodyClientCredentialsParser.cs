using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    public class PostBodyClientCredentialsParser : IClientCredentialsParser
    {

        private readonly IdentityServerOptions _options;

        public PostBodyClientCredentialsParser(IdentityServerOptions options)
        {
            _options = options;
        }

        public string AuthenticationMethod => TokenEndpointAuthMethods.PostBody;

        public async Task<ClientCredentials> ParseAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            var clientId = form["client_id"].FirstOrDefault();
            var credentials = form["client_secret"].FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(clientId))
            {
                throw new InvalidRequestException("Client ID is missing.");
            }
            if (clientId.Length > _options.InputLengthRestrictions.ClientId)
            {
                throw new InvalidRequestException("Client ID exceeds maximum length.");
            }
            if (credentials.Length > _options.InputLengthRestrictions.ClientSecret)
            {
                throw new InvalidRequestException("Client secret exceeds maximum length.");
            }
            if (string.IsNullOrEmpty(credentials))
            {
                return new ClientCredentials(clientId, credentials, ClientSecretTypes.NoSecret);
            }
            else
            {
                return new ClientCredentials(clientId, credentials, ClientSecretTypes.SharedSecret);
            }
        }
    }
}
