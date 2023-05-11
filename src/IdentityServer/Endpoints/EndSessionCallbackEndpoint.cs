using System.Collections.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer.Endpoints
{
    internal class EndSessionCallbackEndpoint : EndpointBase
    {
        private readonly IdentityServerOptions _options;
        private readonly ILoggerFormater _loggerFormater;
        private readonly ISessionManager _sessionManager;

        public EndSessionCallbackEndpoint(
            IdentityServerOptions options,
            ILoggerFormater loggerFormater,
            ISessionManager sessionManager)
        {
            _options = options;
            _loggerFormater = loggerFormater;
            _sessionManager = sessionManager;
        }

        public override async Task<IEndpointResult> HandleAsync(HttpContext context)
        {
            await _loggerFormater.LogRequestAsync();
            
            if (!HttpMethods.IsGet(context.Request.Method))
            {
                return MethodNotAllowed();
            }
            var parameters = context.Request.Query.AsNameValueCollection();

            await _sessionManager.SignOutAsync(_options.AuthenticationScheme);
            var redirectUri = BuildRedirectUri(parameters);

            return Redirect(redirectUri);
        }

        private string BuildRedirectUri(NameValueCollection parameters)
        {
            var state = parameters[OpenIdConnectParameterNames.State];
            var postLogoutRedirectUri = parameters[OpenIdConnectParameterNames.PostLogoutRedirectUri];
            var values = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(state))
            {
                values.Add(OpenIdConnectParameterNames.State, state);
            }
            var querySting = string.Join("&", values.Select(s => $"{s.Key}={s.Value}"));
            return $"{postLogoutRedirectUri}?{querySting}";
        }

    }
}
