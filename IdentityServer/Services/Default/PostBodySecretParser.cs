using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Services
{
    public class PostBodySecretParser : IClientSecretParser
    {

        private readonly IdentityServerOptions _options;

        public PostBodySecretParser(IdentityServerOptions options)
        {
            _options = options;
        }

        public string AuthenticationMethod => TokenEndpointAuthMethods.PostBody;

        public async Task<ClientSecret> ParseAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            var id = form["client_id"].FirstOrDefault();
            var secret = form["client_secret"].FirstOrDefault();
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidRequestException("Client ID is missing.");
            }
            if (id.Length > _options.InputLengthRestrictions.ClientId)
            {
                throw new InvalidRequestException("Client ID exceeds maximum length.");
            }
            if (secret != null && secret.Length > _options.InputLengthRestrictions.ClientSecret)
            {
                throw new InvalidRequestException("Client secret exceeds maximum length.");
            }
            return new ClientSecret(id, ClientSecretTypes.SharedSecret, secret);
        }
    }
}
