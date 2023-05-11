using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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

        public async Task<AuthorizationCode> GenerateAsync(AuthorizeGeneratorRequest request)
        {
            return await CreateAuthorizationCodeAsync(request);           
        }

        public async Task<AuthorizationCode> CreateAuthorizationCodeAsync(AuthorizeGeneratorRequest request)
        {
            var code = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var lifetime = request.Client.AuthorizeCodeLifetime;
            var expirationTime = creationTime.AddSeconds(lifetime);
            var claims = request.Subject.Claims.ToArray();
            var state = request.Body[OpenIdConnectParameterNames.State];
            var scope = request.Body[OpenIdConnectParameterNames.Scope];
            var none = request.Body[OpenIdConnectParameterNames.Nonce];
            var redirectUri = request.Body[OpenIdConnectParameterNames.RedirectUri];
            var responseType = request.Body[OpenIdConnectParameterNames.ResponseType];
            var responseMode = request.Body[OpenIdConnectParameterNames.ResponseMode];
            var codeChallenge = request.Body["code_challenge"];
            var codeChallengeMethod = request.Body["code_challenge_method"];
            var authorizationCode = new AuthorizationCode(
                code: code,
                none: none!,
                state: state!,
                scope: scope!,
                claims: claims,
                lifetime: lifetime,
                redirectUri: redirectUri!,
                responseType: responseType!,
                responseMode: responseMode,
                codeChallenge: codeChallenge,
                codeChallengeMethod: codeChallengeMethod,
                clientId: request.Client.ClientId,
                expirationTime: expirationTime,
                creationTime: creationTime);
            await _store.SaveAuthorizationCodeAsync(authorizationCode);
            return authorizationCode;
        }
    }
}
