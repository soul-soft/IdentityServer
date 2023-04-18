using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IdentityServer.Authentication
{
    internal class OAuth2IntrospectionHandler
        : AuthenticationHandler<OAuth2IntrospectionOptions>
    {
        private OpenIdConnectConfiguration? _configuration;

        public OAuth2IntrospectionHandler(
            IOptionsMonitor<OAuth2IntrospectionOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected new OAuth2IntrospectionEvents Events
        {
            get => (OAuth2IntrospectionEvents)base.Events!;
            set => base.Events = value;
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new OAuth2IntrospectionEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? token;
            var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);
            await Events.MessageReceivedAsync(messageReceivedContext);
            if (messageReceivedContext.Result != null)
            {
                return messageReceivedContext.Result;
            }
            token = messageReceivedContext.Token;
            if (string.IsNullOrEmpty(token))
            {
                string authorization = Request.Headers.Authorization.ToString();

                // If no authorization header found, nothing to process further
                if (string.IsNullOrEmpty(authorization))
                {
                    return AuthenticateResult.NoResult();
                }

                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization.Substring("Bearer ".Length).Trim();
                }

                // If no token found, no further work possible
                if (string.IsNullOrEmpty(token))
                {
                    return AuthenticateResult.NoResult();
                }
            }
            if (_configuration == null && Options.ConfigurationManager != null)
            {
                _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
            }
            var introspectionResponse = await TokenIntrospectionAsync(token);
            if (!introspectionResponse.IsActive)
            {
                return AuthenticateResult.Fail("Token is not active.");
            }
            else
            {
                var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options);
                tokenValidatedContext.Claims = introspectionResponse.Claims;
                await Events.TokenValidatedAsync(tokenValidatedContext);
                if (tokenValidatedContext.Result != null)
                {
                    return tokenValidatedContext.Result;
                }
                var principal = new ClaimsPrincipal(new ClaimsIdentity(introspectionResponse.Claims, Scheme.Name, Options.NameClaimType, Options.RoleClaimType));
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name);
                if (Options.SaveToken)
                {
                    ticket.Properties.StoreTokens(new AuthenticationToken[]
                    {
                        new AuthenticationToken
                        {
                            Name = "access_token",
                            Value = token
                        }
                    });
                }
                return AuthenticateResult.Success(ticket);
            }
        }

        private async Task<IntrospectionResponse> TokenIntrospectionAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _configuration!.IntrospectionEndpoint);
            var parameters = new Dictionary<string, string>
            {
                { OpenIdConnectParameterNames.ClientId, Options.ClientId },
                { OpenIdConnectParameterNames.ClientSecret, Options.ClientSecret },
                { "token", token }
            };
            request.Content = new FormUrlEncodedContent(parameters);
            var response = await Options.Backchannel.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return new IntrospectionResponse(content);
        }
    }
}
