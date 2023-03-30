using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalAuthenticationExtensions
    {
        public static AuthenticationBuilder AddIdentityServer(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<LocalAuthenticationOptions, LocalAuthenticationHandler>(
                LocalAuthenticationDefaults.Scheme,
                LocalAuthenticationDefaults.DisplayName,
                configure => { });
        }

        public static AuthenticationBuilder AddIdentityServer(this AuthenticationBuilder builder, string scheme, string displayName, Action<LocalAuthenticationOptions> configure)
        {
            return builder.AddScheme<LocalAuthenticationOptions, LocalAuthenticationHandler>(scheme, displayName, configure);
        }
    }
}
