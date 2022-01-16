using IdentityServer.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class JwtBearerExtensions
    {
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(
                LocalApi.AuthenticationScheme,
                LocalApi.AuthenticationScheme,
                configure => { });
        }
       
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder, Action<IdentityServerAuthenticationOptions> configure)
        {
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(
                LocalApi.AuthenticationScheme,
                LocalApi.AuthenticationScheme,
                configure);
        }
    }
}
