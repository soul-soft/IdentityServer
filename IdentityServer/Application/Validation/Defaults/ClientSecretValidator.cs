using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class ClientSecretValidator 
        : IClientSecretValidator
    {
        private readonly IClientStore _clients;
        private readonly ISecretsListParser _parsers;
        private readonly ISecretsListValidator _validator;

        public ClientSecretValidator(
            IClientStore clients,
            ISecretsListParser parsers,
            ISecretsListValidator validator)
        {
            _clients = clients;
            _parsers = parsers;
            _validator = validator;
        }

        public async Task<ClientSecretValidationResult> ValidateAsync(HttpContext context)
        {
            var parsedSecret = await _parsers.ParseAsync(context);
            if (parsedSecret == null)
            {
                return Error("No client identifier found");
            }
            //Validate Client
            var client = await _clients.FindClientByIdAsync(parsedSecret.Id);
            if (client == null || !client.Enabled)
            {
                return Error("No client with id '{clientId}' found. aborting", parsedSecret.Id);
            }
            //Validate Secret
            if (!client.RequireClientSecret)
            {
                return Success(client, parsedSecret);
            }
            else
            {
                var secretValidationResult = await _validator.ValidateAsync(client.ClientSecrets, parsedSecret);
                if (secretValidationResult.IsError)
                {
                    return Error("Client secret validation failed for client: {clientId}.", client.ClientId);
                }
                return Success(client, parsedSecret);
            }
        }

        private ClientSecretValidationResult Error(string format, params object?[] args)
        {
            var result = new ClientSecretValidationResult();
            result.Error(string.Format(format, args));
            return result;
        }

        private ClientSecretValidationResult Success(Client client, ParsedSecret secret)
        {
            var result = new ClientSecretValidationResult(client, secret);
            result.Success();
            return result;
        }
    }
}
