using Microsoft.AspNetCore.Http;
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

        #region IReferenceTokenStore
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

        #region IAuthorizeCodeStore
        public static IIdentityServerBuilder AddAuthorizeCodeStore<T>(this IIdentityServerBuilder builder)
          where T : class, IAuthorizeCodeStore
        {
            builder.Services.TryAddTransient<IAuthorizeCodeStore, T>();
            return builder;
        }
        public static IIdentityServerBuilder AddAuthorizeCodeStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, IAuthorizeCodeStore
        {
            builder.Services.TryAddTransient<IAuthorizeCodeStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region InMemoryStoreBuilder
        public static IIdentityServerBuilder AddInMemoryStore(this IIdentityServerBuilder builder, Action<InMemoryStoreBuilder> setup)
        {
            var inMemoryStoreBuilder = new InMemoryStoreBuilder();
            setup(inMemoryStoreBuilder);
            inMemoryStoreBuilder.Build(builder);
            builder.Services.AddDistributedMemoryCache();
            return builder;
        }
        #endregion

        #region IEndpoint
        public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, string name, string path)
            where T : class, IEndpointHandler
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(new EndpointDescriptor(name, path, typeof(T)));
            return builder;
        }
        #endregion

        #region Profilefile
        public static IIdentityServerBuilder AddProfileService<T>(this IIdentityServerBuilder builder)
           where T : class, IProfileService
        {
            builder.Services.AddOrUpdateTransient<IProfileService, T>();
            return builder;
        }
        #endregion

        #region ClientCredentialsRequestValidator
        public static IIdentityServerBuilder AddClientCredentialsRequestValidator<T>(this IIdentityServerBuilder builder)
           where T : class, IClientCredentialsRequestValidator
        {
            builder.Services.AddOrUpdateTransient<IClientCredentialsRequestValidator, T>();
            return builder;
        }
        #endregion

        #region RefreshTokenRequestValidator
        public static IIdentityServerBuilder AddRefreshTokenRequestValidator<T>(this IIdentityServerBuilder builder)
           where T : class, IRefreshTokenRequestValidator
        {
            builder.Services.AddOrUpdateTransient<IRefreshTokenRequestValidator, T>();
            return builder;
        }
        #endregion

        #region ResourceOwnerCredentialRequestValidator
        public static IIdentityServerBuilder AddResourceOwnerCredentialRequestValidator<T>(this IIdentityServerBuilder builder)
           where T : class, IResourceOwnerCredentialRequestValidator
        {
            builder.Services.AddOrUpdateTransient<IResourceOwnerCredentialRequestValidator, T>();
            return builder;
        }
        #endregion

        #region IAuthorizeCodeRequestValidator
        public static IIdentityServerBuilder AddAuthorizeCodeRequestValidator<T>(this IIdentityServerBuilder builder)
        where T : class, IAuthorizeCodeRequestValidator
        {
            builder.Services.AddOrUpdateTransient<IAuthorizeCodeRequestValidator, T>();
            return builder;
        }
        public static IIdentityServerBuilder AddAuthorizeCodeRequestValidator(this IIdentityServerBuilder builder)
        {
            builder.Services.AddOrUpdateTransient<IAuthorizeCodeRequestValidator, AuthorizeCodeRequestValidator>();
            return builder;
        }
        #endregion

        #region ExtensionGrantValidator
        public static IIdentityServerBuilder AddExtensionGrantValidator<T>(this IIdentityServerBuilder builder)
           where T : class, IExtensionGrantValidator
        {
            builder.Services.AddOrUpdateTransient<IExtensionGrantValidator, T>();
            return builder;
        }
        #endregion
    }
}
