using IdentityServer.Configuration;
using IdentityServer.Configuration.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerServiceCollectionExtensions
    {
        public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
        {
            return new IdentityServerBuilder(services);
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();

            builder.AddRequiredPlatformServices()
                .AddCoreServices()
                .AddCookieAuthentication()
                .AddPluggableServices()
                .AddResponseGenerators()
                .AddDefaultEndpoints();

            return builder;
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServer();
        }
    }
}
