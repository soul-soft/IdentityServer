using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections.Specialized;
using System.Net;
using System.Reflection.Emit;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeEndpoint : EndpointBase
    {
        private readonly IClientStore _clientStore;
        private readonly IClaimService _claimService;
        private readonly IdentityServerOptions _options;
        private readonly IResourceValidator _resourceValidator;
        private readonly IAuthorizeResponseGenerator _generator;

        public AuthorizeEndpoint(
            IClientStore clientStore,
            IClaimService claimService,
            IdentityServerOptions options,
            IResourceValidator resourceValidator,
            IAuthorizeResponseGenerator generator)
        {
            _options = options;
            _clientStore = clientStore;
            _claimService = claimService;
            _resourceValidator = resourceValidator;
            _generator = generator;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Validate Request
            NameValueCollection parameters;
            if (HttpMethods.IsGet(context.Request.Method))
            {
                parameters = context.Request.Query.AsNameValueCollection();
            }
            else if (HttpMethods.IsPost(context.Request.Method))
            {
                if (!context.Request.HasFormContentType)
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }
                parameters = context.Request.Form.AsNameValueCollection();
            }
            else
            {
                return MethodNotAllowed();
            }
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
            if (string.IsNullOrEmpty(state))
            {
                return BadRequest(ValidationErrors.InvalidState, "State is too missing");
            }
            #endregion

            #region None
            var none = parameters[OpenIdConnectParameterNames.Nonce];
            #endregion

            #region ResponseMode
            var responseMode = parameters[OpenIdConnectParameterNames.ResponseMode];
            #endregion

            #region ResponseType
            var responseType = parameters[OpenIdConnectParameterNames.ResponseType];
            if (responseType == null)
            {
                return BadRequest(ValidationErrors.InvalidScope, "responseType is null or empty");
            }
            #endregion

            #region Authenticate
            var result = await context.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return Challenge();
            }
            else
            {
                var subject = result.Principal;
                var request = new AuthorizeGeneratorRequest(
                    none: none,
                    scope: scope,
                    state: state,
                    clientId: clientId,
                    redirectUri: redirectUri,
                    responseType: responseType,
                    responseMode: responseMode,
                    client: client,
                    resources: resources,
                    subject: subject);
                var url = await _generator.GenerateAsync(request);
                return Redirect(url);
            }
            #endregion
        }
    }
}
