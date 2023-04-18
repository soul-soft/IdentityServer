using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerAuthenticationExtensions
    {
        public static AuthenticationBuilder AddIdentityServer(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(
                IdentityServerAuthenticationDefaults.AuthenticationScheme,
                IdentityServerAuthenticationDefaults.DisplayName,
                configure => { });
        }
       
        public static AuthenticationBuilder AddIdentityServer(this AuthenticationBuilder builder, string scheme)
        {
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(scheme, IdentityServerAuthenticationDefaults.DisplayName, configure => { });
        }
      
        public static AuthenticationBuilder AddIdentityServer(this AuthenticationBuilder builder, string scheme, string displayName, Action<IdentityServerAuthenticationOptions> configure)
        {
            return builder.AddScheme<IdentityServerAuthenticationOptions, IdentityServerAuthenticationHandler>(scheme, displayName, configure);
        }
    }
}
