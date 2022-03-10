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
                return new ClientCredentials(clientId, credentials, ClientSecretTypes.NoSecret);
            }
            else
            {
                return new ClientCredentials(clientId, credentials, ClientSecretTypes.SharedSecret);
            }
        }
    }
}
