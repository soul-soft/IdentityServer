using IdentityServer.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class JwtBearerExtensions
    {
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<LocalApiAuthenticationOptions, LocalApiAuthenticationHandler>(
                LocalApi.AuthenticationScheme,
                LocalApi.AuthenticationScheme,
                configure =>
                {

                });
        }
    }
}
