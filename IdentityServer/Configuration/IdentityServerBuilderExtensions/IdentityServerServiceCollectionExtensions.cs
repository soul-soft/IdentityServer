using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerServiceCollectionExtensions
    {

        public static IServiceCollection ReplaceTransient<TService, TImplementation>(this IServiceCollection services)
           where TService : class where TImplementation : class, TService
        {
            services.Replace(ServiceDescriptor.Transient<TService, TImplementation>());
            return services;
        }

        public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
        {
            return new IdentityServerBuilder(services);
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();

            builder
                .AddRequiredPlatformServices()
                .AddValidators()
                .AddLocalAuthentication()
                .AddPluggableServices()
                .AddDefaultEndpoints()
                .AddResponseGenerators();

            return builder;
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServer();
        }
    }
}
