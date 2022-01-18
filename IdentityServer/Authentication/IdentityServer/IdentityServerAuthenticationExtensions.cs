using IdentityServer;
using IdentityServer.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class JwtBearerExtensions
    {
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder)
        {           
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(
                IdentityServerDefaults.AuthenticationScheme,
                IdentityServerDefaults.AuthenticationScheme,
                configure => { });
        }
    }
}
