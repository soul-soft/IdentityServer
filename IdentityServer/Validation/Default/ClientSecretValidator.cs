using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public class ClientSecretValidator : IClientSecretValidator
    {
        private readonly IClientStore _clients;
        private readonly ISecretListParser _secretListParser;
        private readonly ISecretListValidator _secretListValidator;

        public ClientSecretValidator(
            IClientStore clients,
            ISecretListParser secretListParser,
            ISecretListValidator secretListValidator)
        {
            _clients = clients;
            _secretListParser = secretListParser;
            _secretListValidator = secretListValidator;
        }

        public async Task<Client> ValidateAsync(HttpContext context)
        {
            var parsedSecret = await _secretListParser.ParseAsync(context);
            var client = await _clients.FindByClientIdAsync(parsedSecret.ClientId);
            if (client == null)
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidClient, "Invalid client credentials");
            }
            if (client.RequireClientSecret)
            {
                await _secretListValidator.ValidateAsync(parsedSecret, client.ClientSecrets);
            }
            return client;
        }
    }
}
