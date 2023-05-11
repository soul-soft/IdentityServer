using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class EndSessionEndpoint : EndpointBase
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly ILoggerFormater _loggerFormater;
        private readonly IIdentityServerUrl _identityServerUrl;

        public EndSessionEndpoint(
            ITokenValidator tokenValidator,
            ILoggerFormater loggerFormater,
            IIdentityServerUrl identityServerUrl)
        {
            _tokenValidator = tokenValidator;
            _loggerFormater = loggerFormater;
            _identityServerUrl = identityServerUrl;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            #region Logger Request
            await _loggerFormater.LogRequestAsync();
            #endregion

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
            var postLogoutRedirectUri = parameters[OpenIdConnectParameterNames.PostLogoutRedirectUri];
            if (!string.IsNullOrEmpty(postLogoutRedirectUri))
            {
                if (!result.Client.AllowedRedirectUris.Contains(postLogoutRedirectUri))
                {
                    return BadRequest(ValidationErrors.InvalidPostLogoutRedirectUri, "Not allowed redirectUri");
                }
                var state = parameters[OpenIdConnectParameterNames.State];
                var redirectUri = BuildRedirectUri(postLogoutRedirectUri, state);
                return Redirect(redirectUri);
            }
            #endregion

            return StatusCode(HttpStatusCode.OK);
        }

        private string BuildRedirectUri(string postLogoutRedirectUri, string? state)
        {
            var address = _identityServerUrl.GetEndpointUri(IdentityServerEndpointNames.EndSessionCallback);
            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(state))
            {
                parameters.Add(OpenIdConnectParameterNames.State, state);
            }
            parameters.Add(OpenIdConnectParameterNames.PostLogoutRedirectUri, WebUtility.UrlEncode(postLogoutRedirectUri));
            var querySting = string.Join("&", parameters.Select(s => $"{s.Key}={s.Value}"));
            return $"{address}?{querySting}";
        }
    }
}
