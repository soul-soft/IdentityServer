using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly ITokenParser _tokenParser;
        private readonly IResourceStore _resourceStore;
        private readonly ITokenValidator _tokenValidator;
        private readonly IUserInfoResponseGenerator _generator;

        public UserInfoEndpoint(
            ITokenParser tokenParser,
            IResourceStore resourceStore,
            ITokenValidator tokenValidator,
            IUserInfoResponseGenerator generator)
        {
            _generator = generator;
            _tokenParser = tokenParser;
            _resourceStore = resourceStore;
            _tokenValidator = tokenValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var token = await _tokenParser.ParserAsync(context);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(OpenIdConnectValidationErrors.InvalidRequest, "Token is missing");
            }
            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (tokenValidationResult.IsError)
            {
                return Unauthorized(tokenValidationResult.Error, tokenValidationResult.ErrorDescription);
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(tokenValidationResult.Claims, "UserInfo"));
            if (!subject.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                return Unauthorized(OpenIdConnectValidationErrors.InsufficientScope, $"Token contains no sub claim");
            }
            var scopes = subject.FindAll(JwtClaimTypes.Scope).Select(s => s.Value);
            var client = tokenValidationResult.Client;
            var resources = await _resourceStore.FindResourcesByScopesAsync(scopes);
            var response = await _generator.ProcessAsync(subject, client, resources);
            return new UserInfoResult(response);
        }
    }
}
