using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalAuthenticationExtensions
    {
        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<IdentityServer.Hosting.LocalAuthenticationOptions, LocalAuthenticationHandler>(
                LocalAuthenticationDefaults.Scheme,
                LocalAuthenticationDefaults.DisplayName,
                configure => { });
        }

        public static AuthenticationBuilder AddLoaclApiAuthentication(this AuthenticationBuilder builder, string scheme, string displayName, Action<IdentityServer.Hosting.LocalAuthenticationOptions> configure)
        {
            return builder.AddScheme<IdentityServer.Hosting.LocalAuthenticationOptions, LocalAuthenticationHandler>(scheme, displayName, configure);
        }
    }
}
