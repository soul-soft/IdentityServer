using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Endpoints
{
    internal class AuthorizeResponseGenerator : IAuthorizeResponseGenerator
    {
        private readonly ISystemClock _clock;
        private readonly IAuthorizationCodeStore _store;
        private readonly IRandomGenerator _randomGenerator;

        public AuthorizeResponseGenerator(
            ISystemClock clock,
            IAuthorizationCodeStore store,
            IRandomGenerator randomGenerator)
        {
            _clock = clock;
            _store = store;
            _randomGenerator = randomGenerator;
        }

        public async Task<string> GenerateAsync(AuthorizeGeneratorRequest request)
        {
            var code = await CreateAuthorizationCodeAsync(request);
            var buffer = new StringBuilder();
            buffer.Append(request.RedirectUri);
            buffer.AppendFormat("?{0}={1}", OpenIdConnectParameterNames.Code, code);
            buffer.AppendFormat("&{0}={1}", OpenIdConnectParameterNames.State, request.State);
            return buffer.ToString();
        }

        public async Task<string> CreateAuthorizationCodeAsync(AuthorizeGeneratorRequest request)
        {
            var code = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var lifetime = request.Client.AuthorizeCodeLifetime;
            var expirationTime = creationTime.AddSeconds(lifetime);
            var claims = request.Subject.Claims.ToArray();
            var authorizationCode = new AuthorizationCode(
                code: code,
                claims: claims,
                state: request.State,
                scope: request.Scope,
                none: request.None,
                clientId: request.Client.ClientId,
                redirectUri: request.RedirectUri,
                responseType: request.ResponseType,
                responseMode: request.ResponseMode,
                lifetime: lifetime,
                expirationTime: expirationTime,
                creationTime: creationTime);
            await _store.SaveAuthorizationCodeAsync(authorizationCode);
            return authorizationCode.Code;
        }
    }
}
