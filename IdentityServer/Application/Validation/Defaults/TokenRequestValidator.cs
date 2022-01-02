using System.Collections.Specialized;
using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Models;
using IdentityServer.Storage;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Application
{
    internal class TokenRequestValidator
        : ITokenRequestValidator
    {
        private readonly IdentityServerOptions _options;
        private readonly IClientStore _clients;
        private readonly ISecretsListParser _clientProvider;
        private readonly IClientSecretValidator _clientValidator;

        public TokenRequestValidator(
            IdentityServerOptions options,
            IClientStore clients,
            ISecretsListParser clientProvider,
            IClientSecretValidator clientValidator)
        {
            _options = options;
            _clients = clients;
            _clientProvider = clientProvider;
            _clientValidator = clientValidator;
        }

        public async Task ValidateRequestAsync(TokenRequestValidationContext context)
        {
            var parameters = await context.HttpContext.Request.ReadFormAsNameValueCollectionAsync();
            //get grantType
            var grantType = parameters.Get(OidcConstants.TokenRequest.GrantType);
            if (string.IsNullOrWhiteSpace(grantType))
            {
                context.Fail(OidcConstants.TokenErrors.UnsupportedGrantType);
                return;
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                context.Fail(OidcConstants.TokenErrors.UnsupportedGrantType);
                return;
            }
            //parse Secret
            var parsedClient = await _clientProvider.ParseAsync(context.HttpContext);
            if (parsedClient == null)
            {
                context.Fail(OidcConstants.TokenErrors.InvalidRequest);
                return;
            }
            //
            //find Client
            var client = await _clients.FindClientByIdAsync(parsedClient.Id);
            if (client == null)
            {
                context.Fail(OidcConstants.TokenErrors.InvalidClient);
                return;
            }
            //validation client
            var clientValidationContext = new ClientSecretValidationContext(client, parsedClient);
            await _clientValidator.ValidateAsync(clientValidationContext);
            if (clientValidationContext.IsError)
            {
                context.Fail(OidcConstants.TokenErrors.UnauthorizedClient);
            }
            switch (grantType)
            {
                default:
                    await RunValidationAsync(ValidateClientCredentialsRequestAsync);
                    break;
            }
        }

        private async Task RunValidationAsync(Func<NameValueCollection, Task> func, NameValueCollection parameters)
        {
            await func(parameters);
        }

        private async Task ValidateClientCredentialsRequestAsync(NameValueCollection parameters)
        {
            
        }
    }
}
