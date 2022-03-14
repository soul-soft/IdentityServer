using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class IntrospectionEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly SecretValidatorCollection _secretValidators;
        private readonly SecretParserCollection _secretParsers;

        public IntrospectionEndpoint(
            IClientStore clients,
            IResourceStore resources,
            SecretParserCollection secretParsers,
            SecretValidatorCollection secretValidators)
        {
            _clients = clients;
            _resources = resources;
            _secretParsers = secretParsers;
            _secretValidators = secretValidators;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            #region Validate Method
            if (!HttpMethods.IsPost(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            if (!context.Request.HasFormContentType)
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Invalid contextType");
            }
            #endregion

            #region Validate Credentials
            var credentials = await _secretParsers.ParseAsync(context);
            if (credentials.Type == ClientSecretTypes.NoSecret)
            {
                return BadRequest(OpenIdConnectErrors.InvalidRequest, "Client credentials is missing");
            }
            var client = await _clients.FindByClientIdAsync(credentials.ClientId);
            if (client == null)
            {
                return BadRequest(OpenIdConnectErrors.InvalidClient, "Invalid client credentials");
            }
            await _secretValidators.ValidateAsync(credentials, client.ClientSecrets);
            #endregion

            var apis = await _resources.FindApiResourcesByNameAsync(credentials.ClientId);

            return MethodNotAllowed();
        }
    }
}
