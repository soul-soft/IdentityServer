using IdentityServer.Hosting.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdditionalIdentityServerBuilderExtensions
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

        #region ICacheStore
        public static IIdentityServerBuilder AddCacheStore(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<ICacheStore, CacheStore>();
            return builder;
        }
        public static IIdentityServerBuilder AddCacheStore<T>(this IIdentityServerBuilder builder)
            where T : class, ICacheStore
        {
            builder.Services.TryAddTransient<ICacheStore, T>();
            return builder;
        }
        public static IIdentityServerBuilder AddCacheStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, ICacheStore
        {
            builder.Services.TryAddTransient<ICacheStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region ITokenStore
        public static IIdentityServerBuilder AddTokenStore(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<ITokenStore, TokenStore>();
            return builder;
        }
        public static IIdentityServerBuilder AddTokenStore<T>(this IIdentityServerBuilder builder)
          where T : class, ITokenStore
        {
            builder.Services.TryAddTransient<ITokenStore, T>();
            return builder;
        }
        public static IIdentityServerBuilder AddTokenStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, ITokenStore
        {
            builder.Services.TryAddTransient<ITokenStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region IAuthorizeCodeStore
        public static IIdentityServerBuilder AddAuthorizationCodeStore(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IAuthorizationCodeStore, AuthorizationCodeStore>();
            return builder;
        }
        public static IIdentityServerBuilder AddAuthorizationCodeStore<T>(this IIdentityServerBuilder builder)
          where T : class, IAuthorizationCodeStore
        {
            builder.Services.TryAddTransient<IAuthorizationCodeStore, T>();
            return builder;
        }
        public static IIdentityServerBuilder AddAuthorizationCodeStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, IAuthorizationCodeStore
        {
            builder.Services.TryAddTransient<IAuthorizationCodeStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region ISigningCredentialsStore
        public static IIdentityServerBuilder AddSigningCredentialsStore<T>(this IIdentityServerBuilder builder)
            where T : class, ISigningCredentialsStore
        {
            builder.Services.TryAddTransient<ISigningCredentialsStore, T>();
            return builder;
        }

        public static IIdentityServerBuilder AddSigningCredentialsStore<T>(this IIdentityServerBuilder builder, Func<IServiceProvider, T> implementationFactory)
            where T : class, ISigningCredentialsStore
        {
            builder.Services.TryAddTransient<ISigningCredentialsStore>(implementationFactory);
            return builder;
        }
        #endregion

        #region IEndpoint
        public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, string name, string path)
            where T : class, IEndpointHandler
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IdentityServerOptions>();
                if (!path.StartsWith('/'))
                {
                    path = $"{options.Endpoints.PathPrefix}/{path}";
                }
                return new EndpointDescriptor(name, path, typeof(T));
            });
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
