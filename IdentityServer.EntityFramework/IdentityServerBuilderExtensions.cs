using IdentityServer.EntityFramework;
using IdentityServer.EntityFramework.Stores;
using IdentityServer.Hosting.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddEntityFrameworkStores(this IIdentityServerBuilder builder, Action<EntityFrameworkStoreOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.AddTokenStore<TokenStore>();
            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            builder.AddAuthorizationCodeStore<AuthorizationCodeStore>();
            return builder;
        }

        public static IIdentityServerBuilder AddEntityFrameworkStore(this IIdentityServerBuilder builder)
        {
            builder.AddEntityFrameworkStores(configureOptions => { });
            return builder;
        }
    }
}
