using IdentityServer;
using IdentityServer.Endpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreExtensions
    {
        #region CoreServices
        internal static IIdentityServerBuilder AddCoreServices(this IIdentityServerBuilder builder)
        {
            return builder;
        }
        #endregion       

        #region PlatformServices
        /// <summary>
        /// 必要的平台服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal static IIdentityServerBuilder AddRequiredPlatformServices(this IIdentityServerBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<IdentityServerOptions>>().Value);
            builder.Services.AddHttpClient();
            return builder;
        }
        #endregion

        #region PluggableServices
        public static IIdentityServerBuilder AddPluggableServices(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IServerUrl, ServerUrl>();
            builder.Services.TryAddTransient<IIdGenerator, IdGenerator>();
            builder.Services.TryAddTransient<ISecretParser, PostBodySecretParser>();
            builder.Services.TryAddTransient<ISecretsParser, SecretParsers>();
            builder.Services.TryAddTransient<ITokenService, TokenService>();
            builder.Services.TryAddTransient<ITokenCreationService, TokenCreationService>();
            builder.Services.TryAddTransient<IRefreshTokenService, RefreshTokenService>();
            builder.Services.TryAddTransient<IReferenceTokenService, ReferenceTokenService>();
            return builder;
        }
        #endregion

        #region Validators
        public static IIdentityServerBuilder AddValidators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IScopeValidator, ScopeValidator>();
            builder.Services.TryAddTransient<IResourceValidator, ResourceValidator>();
            builder.Services.TryAddTransient<IGrantTypeValidator, GrantTypeValidator>();
            builder.Services.TryAddTransient<ISecretsValidator, SecretsValidator>();
            builder.Services.TryAddTransient<ISecretValidator, SharedSecretValidator>();
            return builder;
        }
        #endregion

        #region Endpoints
        public static IIdentityServerBuilder AddEndpoint<T>(this IIdentityServerBuilder builder, string name, PathString path)
          where T : class, IEndpointHandler
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(new EndpointDescriptor(name, path, typeof(T)));
            return builder;
        }

        internal static IIdentityServerBuilder AddDefaultEndpoints(this IIdentityServerBuilder builder)
        {
            builder.AddEndpoint<TokenEndpoint>(Constants.EndpointNames.Token, Constants.EndpointRoutePaths.Token);
            builder.AddEndpoint<TokenEndpoint>(Constants.EndpointNames.Authorize, Constants.EndpointRoutePaths.Authorize);
            builder.AddEndpoint<UserInfoEndpoint>(Constants.EndpointNames.UserInfo, Constants.EndpointRoutePaths.UserInfo);
            builder.AddEndpoint<DiscoveryEndpoint>(Constants.EndpointNames.Discovery, Constants.EndpointRoutePaths.Discovery);
            builder.AddEndpoint<DiscoveryKeyEndpoint>(Constants.EndpointNames.DiscoveryJwks, Constants.EndpointRoutePaths.DiscoveryJwks);
            return builder;
        }
        #endregion

        #region ResponseGenerators
        public static IIdentityServerBuilder AddResponseGenerators(this IIdentityServerBuilder builder)
        {
            builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
            builder.Services.TryAddTransient<ITokenResponseGenerator, TokenResponseGenerator>();
            return builder;
        }
        #endregion
    }
}
