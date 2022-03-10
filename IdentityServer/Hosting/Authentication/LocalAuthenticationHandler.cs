using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IdentityServer.Hosting
{
    internal class LocalAuthenticationHandler
        : AuthenticationHandler<LocalAuthenticationOptions>
    {
        private readonly ITokenParser _tokenParser;
        private readonly ITokenValidator _tokenValidator;

        public LocalAuthenticationHandler(
            ITokenValidator tokenValidator,
            ITokenParser tokenParser,
            IOptionsMonitor<LocalAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _tokenValidator = tokenValidator;
            _tokenParser = tokenParser;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var token = await _tokenParser.ParserAsync(Context);
                if (string.IsNullOrEmpty(token))
                {
                    return AuthenticateResult.NoResult();
                }
                var claims = await _tokenValidator.ValidateAccessTokenAsync(token);
                var properties = new AuthenticationProperties();
                if (Options.SaveToken)
                {
                    properties.StoreTokens(new[]
                    {
                        new AuthenticationToken { Name = "access_token", Value = token }
                    });
                }
                var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
                var ticket = new AuthenticationTicket(subject, properties, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
