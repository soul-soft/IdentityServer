using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityServer.Authentication
{
    internal class LocalApiAuthenticationHandler
        : AuthenticationHandler<LocalApiAuthenticationOptions>
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly IBearerTokenUsageParser _bearerTokenUsageParser;

        public LocalApiAuthenticationHandler(
            ITokenValidator tokenValidator,
            IBearerTokenUsageParser bearerTokenUsageParser,
            IOptionsMonitor<LocalApiAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _tokenValidator = tokenValidator;
            _bearerTokenUsageParser = bearerTokenUsageParser;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = await _bearerTokenUsageParser.ParserAsync(Context);
            if (token == null)
            {
                return AuthenticateResult.NoResult();
            }
            var result = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (result.IsError)
            {
                return AuthenticateResult.Fail("Failed to validate the token");
            }
            var ticket = new AuthenticationTicket(result.Subject, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

    }
}
