using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IdentityServer.Hosting
{
    internal class IdentityServerAuthHandler
        : AuthenticationHandler<IdentityServerAuthOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ITokenValidator _tokenValidator;
        private readonly IBearerTokenUsageParser _bearerTokenUsageParser;

        public IdentityServerAuthHandler(
            ITokenValidator tokenValidator,
            IBearerTokenUsageParser bearerTokenUsageParser,
            ILoggerFactory loggerFactory,
            IOptionsMonitor<IdentityServerAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _loggerFactory = loggerFactory;
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
            try
            {
                var claims = await _tokenValidator.ValidateAsync(token);
                var properties = new AuthenticationProperties();
                if (Options.SaveToken)
                {
                    properties.StoreTokens(new[]
                    {
                        new AuthenticationToken { Name = "access_token", Value = token }
                    });
                }
                var identity = new ClaimsIdentity(Scheme.Name);
                identity.AddClaims(claims);
                var subject = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(subject, properties, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authenticateResult = await AuthenticateAsync();
            if (!authenticateResult.Succeeded)
            {
                var endpoint = Context.GetEndpoint();
                if (endpoint != null && endpoint.IsIdentityEndpoint())
                {
                    var result = new ErrorResult(
                    OpenIdConnectTokenErrors.InvalidToken,
                    authenticateResult.Failure?.Message,
                    HttpStatusCode.Unauthorized);
                    await result.ExecuteAsync(Context);
                    _loggerFactory.CreateLogger("IdentityServer").LogError(authenticateResult.Failure, authenticateResult.Failure?.Message);
                }
            }
        }
    }
}
