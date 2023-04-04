using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeEndpoint : EndpointBase
    {
        private readonly IClientStore _clientStore;
        private readonly IdentityServerOptions _options;
        private readonly IResourceValidator _resourceValidator;

        public AuthorizeEndpoint(
            IdentityServerOptions options,
            IResourceValidator resourceValidator,
            IClientStore clientStore)
        {
            _options = options;
            _clientStore = clientStore;
            _resourceValidator = resourceValidator;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Validate Request
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            #endregion

            #region Read Query
            var parameters = context.Request.Query.AsNameValueCollection();
            #endregion

            #region Validate Client
            var clientId = parameters[OpenIdConnectParameterNames.ClientId];
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ValidationErrors.InvalidScope, "ClientId is missing");
            }
            var client = await _clientStore.FindClientAsync(clientId);
            if (client == null)
            {
                return BadRequest(ValidationErrors.InvalidScope, "ClientId is missing");
            }
            #endregion

            #region GrantType
            if (!client.AllowedGrantTypes.Contains(GrantTypes.AuthorizationCode))
            {
                return BadRequest(ValidationErrors.UnauthorizedClient, $"The client does not allow {GrantTypes.AuthorizationCode}");
            }
            #endregion

            #region RedirectUri
            var redirectUri = parameters[OpenIdConnectParameterNames.RedirectUri];
            if (string.IsNullOrEmpty(redirectUri))
            {
                return BadRequest(ValidationErrors.InvalidRequest, "RedirectUri type is missing");
            }
            if (!client.AllowedRedirectUris.Any(a => a == redirectUri))
            {
                return BadRequest(ValidationErrors.InvalidGrant, "Not allowed redirectUri");
            }
            redirectUri = WebUtility.UrlDecode(redirectUri);
            #endregion

            #region Validate Resources
            var scope = parameters[OpenIdConnectParameterNames.Scope] ?? string.Empty;
            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                return BadRequest(ValidationErrors.InvalidScope, "Scope is too long");
            }
            var scopes = scope.Split(" ").Where(a => !string.IsNullOrWhiteSpace(a));
            var resources = await _resourceValidator.ValidateAsync(client, scopes);
            #endregion

            #region State
            var state = parameters[OpenIdConnectParameterNames.State];
            #endregion

            #region ResponseType
            var responseType = parameters[OpenIdConnectParameterNames.ResponseType];
            if (responseType == null)
            {
                responseType = "";
            }
            #endregion

            #region Authenticate
            var result = await context.AuthenticateAsync();
            if (result.Succeeded)
            {
                var request = new AuthorizeGeneratorRequest(state,redirectUri,responseType,client,resources,_options);
                return AuthorizeEndpointResult(request);
            }
            else
            {
                return Challenge();
            }
            #endregion
        }
    }
}
