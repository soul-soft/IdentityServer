using IdentityServer.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IdentityServerAuthenticationExtensions
    {
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder)
        {           
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(
                IdentityServerAuthenticationDefaults.AuthenticationScheme,
                IdentityServerAuthenticationDefaults.AuthenticationScheme,
                configure => { });
        }
    }
}
