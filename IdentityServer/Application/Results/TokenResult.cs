using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public class TokenResult : IEndpointResult
    {
        public readonly TokenResponse _token;

        public TokenResult(TokenResponse token)
        {
            _token = token;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var data = new Dictionary<string, object?>();
            data.Add("id_token", _token.AccessToken);
            data.Add("access_token", _token.IdentityToken);
            data.Add("refresh_token", _token.RefreshToken);
            data.Add("scope", _token.Scope);
            data.Add("expires_in", _token.ExpiresIn);
            await context.WriteAsJsonExAsync(data);
        }
    }
}
