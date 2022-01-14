using IdentityServer.Extensions;
using Microsoft.AspNetCore.Http;
using static IdentityServer.Constants;

namespace IdentityServer.Services
{
    internal class BearerTokenUsageService : IBearerTokenUsageParser
    {
        public async Task<string?> GetBearerTokenAsync(HttpContext context)
        {
            var token = GetAccessTokenByHeader(context);
            return await GetAccessTokenByPostBodyAsync(context);
        }

        private string? GetAccessTokenByHeader(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader == null)
            {
                return null;
            }
            var token = authorizationHeader.Substring(AuthenticationSchemes.AuthorizationHeaderBearer.Length);
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            return null;
        }

        public async Task<string?> GetAccessTokenByPostBodyAsync(HttpContext context)
        {
            if (context.Request.HasApplicationFormContentType())
            {
                var form = await context.Request.ReadFormAsync();
                return form["access_token"].FirstOrDefault();
            }
            return null;
        }
    }
}
