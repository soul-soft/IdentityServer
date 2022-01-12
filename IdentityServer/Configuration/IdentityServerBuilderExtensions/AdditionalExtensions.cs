using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdditionalExtensions
    {
        #region IClientStore
        public static IIdentityServerBuilder AddClientStore<T>(this IIdentityServerBuilder builder)
            where T : class, IClientStore
        {
            builder.Services.TryAddTransient<IClientStore, T>();
            return builder;
        }
        
        public static IIdentityServerBuilder AddClientStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, IClientStore
        {
            builder.Services.TryAddTransient<IClientStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region IResourceStore
        public static IIdentityServerBuilder AddResourceStore<T>(this IIdentityServerBuilder builder)
            where T : class, IResourceStore
        {
            builder.Services.TryAddTransient<IResourceStore, T>();
            return builder;
        }

        public static IIdentityServerBuilder AddResourceStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, IResourceStore
        {
            builder.Services.TryAddTransient<IResourceStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region ISigningCredentialStore
        public static IIdentityServerBuilder AddSigningCredentialStore<T>(this IIdentityServerBuilder builder)
            where T : class, ISigningCredentialStore
        {
            builder.Services.TryAddTransient<ISigningCredentialStore, T>();
            return builder;
        }

        public static IIdentityServerBuilder AddSigningCredentialStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, ISigningCredentialStore
        {
            builder.Services.TryAddTransient<ISigningCredentialStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region IRefreshTokenStore
        public static IIdentityServerBuilder AddRefreshTokenStore<T>(this IIdentityServerBuilder builder)
            where T : class, IRefreshTokenStore
        {
            builder.Services.TryAddTransient<IRefreshTokenStore,T>();
            return builder;
        }
        public static IIdentityServerBuilder AddRefreshTokenStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, IRefreshTokenStore
        {
            builder.Services.TryAddTransient<IRefreshTokenStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region ReferenceTokenStore
        public static IIdentityServerBuilder AddReferenceTokenStore<T>(this IIdentityServerBuilder builder)
          where T : class, IReferenceTokenStore
        {
            builder.Services.TryAddTransient<IReferenceTokenStore, T>();
            return builder;
        }
        public static IIdentityServerBuilder AddReferenceTokenStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, IReferenceTokenStore
        {
            builder.Services.TryAddTransient<IReferenceTokenStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region InMemoryStores
        public static IIdentityServerBuilder AddInMemoryStores(this IIdentityServerBuilder builder, Action<InMemoryStoreBuilder> configure)
        {
            builder.Services.AddDistributedMemoryCache();
            builder.AddRefreshTokenStore<InMemoryRefreshTokenStore>();
            builder.AddReferenceTokenStore<InMemoryReferenceTokenStore>();
            var inMemoryStoreBuilder = new InMemoryStoreBuilder();
            configure(inMemoryStoreBuilder);
            inMemoryStoreBuilder.Build(builder);
            return builder;
        }
        #endregion
    }
}
