using System.Collections.Specialized;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class EndSessionEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly ITokenValidator _tokenValidator;
        private readonly ISessionManager _sessionManager;

        public EndSessionEndpoint(
            IdentityServerOptions options,
            ITokenValidator tokenValidator,
            ISessionManager sessionManager)
        {
            _options = options;
            _tokenValidator = tokenValidator;
            _sessionManager = sessionManager;
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

            #region Validate IdTokenHint
            var idTokenHint = parameters[OpenIdConnectParameterNames.IdTokenHint];
            if (string.IsNullOrEmpty(idTokenHint))
            {
                return BadRequest(ValidationErrors.InvalidRequest, $"{OpenIdConnectParameterNames.IdTokenHint} is missing");
            }
            #endregion

            #region Validate IdToken
            var result = await _tokenValidator.ValidateIdentityTokenAsync(idTokenHint);
            if (result.IsError)
            {
                return BadRequest(ValidationErrors.InvalidGrant, $"{OpenIdConnectParameterNames.IdTokenHint} validating fait");
            }
            #endregion

            #region Validate PostLogoutRedirectUri
            var client = result.Client;
            var postLogoutRedirectUri = parameters[OpenIdConnectParameterNames.PostLogoutRedirectUri];
            if (!string.IsNullOrEmpty(postLogoutRedirectUri) && !client.AllowedRedirectUris.Contains(postLogoutRedirectUri))
            {
                return BadRequest(ValidationErrors.InvalidGrant, "Not allowed redirectUri");
            }
            #endregion

            var propertities = new AuthenticationProperties();
            await _sessionManager.SignOutAsync(_options.AuthenticationScheme, propertities);
           
            if (!string.IsNullOrEmpty(postLogoutRedirectUri))
            {
                var state = parameters[OpenIdConnectParameterNames.State];
                return Redirect($"{postLogoutRedirectUri}?state={state}");
            }

            return StatusCode(HttpStatusCode.OK);
        }
    }
}
