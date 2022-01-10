using IdentityServer.Configuration;
using IdentityServer.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Additional
    {
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
      
        public static IIdentityServerBuilder AddReferenceTokenStore<T>(this IIdentityServerBuilder builder)
          where T : class, IReferenceTokenStore
        {
            builder.Services.TryAddTransient<IReferenceTokenStore,T>();
            return builder;
        }
        public static IIdentityServerBuilder AddReferenceTokenStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
          where T : class, IReferenceTokenStore
        {
            builder.Services.TryAddTransient<IReferenceTokenStore>(implementationFactory);
            return builder;
        }
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

        public static IIdentityServerBuilder AddInMemoryStores(this IIdentityServerBuilder builder, Action<InMemoryStoreBuilder> configure)
        {
            var inMemoryStoreBuilder = new InMemoryStoreBuilder();
            configure(inMemoryStoreBuilder);
            inMemoryStoreBuilder.Build(builder);
            return builder;
        }
    }
}
