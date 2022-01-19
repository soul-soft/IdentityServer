using IdentityServer;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IdentityServerAuthExtensions
    {
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder)
        {           
            return builder.AddScheme<IdentityServerAuthOptions, IdentityServerAuthHandler>(
                IdentityServerAuthDefaults.Scheme,
                IdentityServerAuthDefaults.DisplayName,
                configure => { });
        }
    }
}
